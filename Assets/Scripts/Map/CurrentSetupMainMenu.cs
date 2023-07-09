using System.Linq;

namespace SkibidiRunner.Map
{
    public class CurrentSetupMainMenu : CurrentSetup
    {
        public override void Initialize()
        {
            CurrentMusic = gameMusics.First();
        }
    }
}