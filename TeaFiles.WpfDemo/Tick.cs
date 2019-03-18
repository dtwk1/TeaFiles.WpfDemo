// copyright discretelogics 2012. released under the gpl v3. see license.txt for details.
namespace TeaFiles.WpfDemo
{
    using TeaTime;

    // define the item type of a TeaFile
    struct Tick
    {
        public Time Time;
        public double Price;
        public int Volume;

        public override string ToString()
        {
            return Time + " " + Price;
        }
    }
}