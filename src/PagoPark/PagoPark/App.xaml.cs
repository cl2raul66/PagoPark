

namespace PagoPark
{
    public partial class App : Application
    {
        private double dHeight, dWidth;
        private Window w;

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
            if (DeviceInfo.Current.Idiom != DeviceIdiom.Phone)
            {
                MainPage.Loaded += MainPage_Loaded;
            }

        }

        private void MainPage_Loaded(object sender, EventArgs e)
        {
            dHeight = DeviceDisplay.Current.MainDisplayInfo.Height;
            dWidth = DeviceDisplay.Current.MainDisplayInfo.Width;

            var newHeight = dHeight - (dHeight * 0.10);
            var newWidth = dWidth - (dWidth * 0.10);

            w.Height = newHeight;
            w.Width = newWidth;

            w.X = (dWidth - newWidth) / 2;
            w.Y = (dHeight * 0.10) / 4;
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            w = base.CreateWindow(activationState);

            //w.Height = 0;
            //w.Width = 0;

            return w;
        }
    }
}