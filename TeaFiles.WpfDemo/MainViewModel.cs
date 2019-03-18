// copyright discretelogics 2012. released under the gpl v3. see license.txt for details.
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TeaTime;

namespace TeaFiles.WpfDemo
{
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Reactive.Bindings;

    class Program
    {
        private const string ConnectionString = "../../Data/1";

        public Program()
        {
            System.IO.Directory.CreateDirectory("../../Data");
            (InsertCommand as ReactiveCommand).Subscribe(Insert);

            this.Ticks = (RefreshCommand as ReactiveCommand).Select(_=>this.Refresh().Select(t=>Mapper.Map(t)).ToArray()).ToReactiveProperty();
        }


        public ReactiveProperty<long> ExecutionTime { get; private set; }=new ReactiveProperty<long>();



        public ReactiveProperty<TickViewModel[]> Ticks { get; private set; } = new ReactiveProperty<TickViewModel[]>();


        public ICommand InsertCommand { get; } = new ReactiveCommand();

        private void Insert()
        {
            {
                try
                {
                    var sw = Stopwatch.StartNew();

                    Time t = DateTime.Now;
                    Task.Run(
                            () =>
                                {
                                    if (!System.IO.File.Exists(ConnectionString))
                                        using (var tf = TeaFile<Tick>.Create(ConnectionString))
                                    {
                                        tf.Write(
                                            Enumerable.Range(1, 100).Select(
                                                i => new Tick
                                                {
                                                    Time = t.AddDays(i),
                                                    Price = i * 101.0,
                                                    Volume = i * 1000
                                                }));
                                    }
                                    else
                                    {
                                        using (var tf = TeaFile<Tick>.Append(ConnectionString))
                                        {
                                            tf.Write(
                                                Enumerable.Range(1, 100).Select(
                                                    i => new Tick
                                                             {
                                                                 Time = t.AddDays(i),
                                                                 Price = i * 101.0,
                                                                 Volume = i * 1000
                                                             }));
                                        }

                                    }
                                })
                        .ContinueWith(x =>
                            {
                                sw.Stop();

                            }).ContinueWith(x => ExecutionTime.Value = sw.ElapsedMilliseconds, TaskScheduler.FromCurrentSynchronizationContext());

                    sw.Stop();



                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public ICommand RefreshCommand { get; } = new ReactiveCommand();


        private Tick[] Refresh()
        {
            IItemCollection<Tick> ticks = null;
            if(System.IO.File.Exists(ConnectionString))
            using (var tf = TeaFile<Tick>.OpenRead(ConnectionString, ItemDescriptionElements.None))
            {
                ticks = tf.Items;
                return ticks.ToArray();
            }

            return null;

        }
    }
}