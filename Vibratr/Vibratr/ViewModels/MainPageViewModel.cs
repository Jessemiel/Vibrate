using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Vibratr.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INotifyPropertyChanged
    {

        public DelegateCommand VibrateDevice { get; set; }
        public DelegateCommand CancelVibrate { get; set; }
        public DelegateCommand Pattern1 { get; set; }
        public DelegateCommand Pattern2 { get; set; }
        public DelegateCommand Pattern3 { get; set; }
        public DelegateCommand NavigatoToAbout { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;


        private double _duration;
        public double Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Duration"));
            }
        }
        

        private bool _allowRun;
        public bool AllowRun
        {
            get { return _allowRun; }
            set
            {
                _allowRun = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AllowRun"));

            }
        }     

        private bool _p1Stat;
        public bool P1Stat
        {
            get { return _p1Stat; }
            set
            {
                _p1Stat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("P1Stat"));

            }
        }
        private bool _p2Stat;
        public bool P2Stat
        {
            get { return _p2Stat; }
            set
            {
                _p2Stat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("P2Stat"));

            }
        }
        private bool _p3Stat;
        public bool P3Stat
        {
            get { return _p3Stat; }
            set
            {
                _p3Stat = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("P3Stat"));
            }
        }



        private CancellationTokenSource CancelSource;
        private CancellationToken CancelToken;

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
            AllowRun = true;
            Duration = 300;    
            VibrateDevice = new DelegateCommand(ExecuteVibrateDevice);
            CancelVibrate = new DelegateCommand(ExecuteCancelVibrate);
            Pattern1 = new DelegateCommand(ExecutePattern1);
            Pattern2 = new DelegateCommand(ExecutePattern2);
            Pattern3 = new DelegateCommand(ExecutePattern3);
            NavigatoToAbout = new DelegateCommand(ExecuteNavigatoToAbout);
        }

        private void ExecuteNavigatoToAbout()
        {
            NavigationService.NavigateAsync("AboutPage");
        }

        private async void ExecuteCancelVibrate()
        {
            P1Stat = false;
            P2Stat = false;
            P3Stat = false;
            Vibration.Cancel();
            await SetCancellation();
            AllowRun = true;
        }

        private async void ExecuteVibrateDevice()
        {
            //Duration = Duration == 0 ? 1 : Duration;            
            //Vibration.Vibrate(Duration);
            P1Stat = false;
            P2Stat = false;
            P3Stat = false;
            await SetCancellation();
            await Task.Run(async () =>
            {
                try
                {
                    AllowRun = false;
                    while(true)
                    {
                        CancelToken.ThrowIfCancellationRequested();
                        await LongShot(Duration, 0);
                        await Task.Delay((int)(Duration*1.5), CancelToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    //log if may nag cancel
                }
                catch (Exception e)
                {

                }
                finally
                {
                    AllowRun = true;                   
                }

            });
        }

        private void ExecutePatternNormal()
        {
           

        }


        
        private async void ExecutePattern1()
        {
            P1Stat = true;
            P2Stat = false;
            P3Stat = false;
            await SetCancellation();
            await Task.Run(async()=> {
                try
                {
                    AllowRun = false;
                    while (true)
                    {
                        CancelToken.ThrowIfCancellationRequested();
                        await Tick(8, 100);
                        await LongShot(500, 500);
                        await Task.Delay(500, CancelToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    //log if may nag cancel
                }
                catch(Exception e)
                {

                }
                finally
                {
                    AllowRun = true;
                    
                }

            });
                   
        }

        private async void ExecutePattern2()
        {
            P1Stat = false;
            P2Stat = true;
            P3Stat = false;
            await SetCancellation();
            await Task.Run(async () => {
                try
                {
                    AllowRun = false;
                    while (true)
                    {
                        CancelToken.ThrowIfCancellationRequested();
                        await LongShot(1000, 500);
                        await Task.Delay(300, CancelToken);
                        await Tick(4, 150);
                        await Task.Delay(300, CancelToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    //log if may nag cancel
                    
                }
                catch (Exception e)
                {

                }
                finally
                {
                    AllowRun = true;
                    
                }

            });
            
        }

        private async void ExecutePattern3()
        {
            P1Stat = false;
            P2Stat = false;
            P3Stat = true;
            await SetCancellation();
            await Task.Run(async () => {
                try
                {
                    AllowRun = false;
                    while (true)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            CancelToken.ThrowIfCancellationRequested();
                            await LongShot(200, 600);
                            await Task.Delay(100, CancelToken);
                        }
                        for (int i = 0; i < 2; i++)
                        {
                            CancelToken.ThrowIfCancellationRequested();
                            await LongShot(800, 800);
                            await Task.Delay(800, CancelToken);
                        }
                    }
                   
                }
                catch (OperationCanceledException)
                {
                    //log if may nag cancel
                }
                catch (Exception e)
                {

                }
                finally
                {
                    AllowRun = true;
                  
                }

            });

            
        }


        private async Task Tick(int count, int duration)
        {
            for (var ctr = 0; ctr < count; ctr++)
            {
                Vibration.Vibrate(50);
                await Task.Delay(duration, CancelToken);
            }
            
        }

        private async Task LongShot(double vibrationDuration, int delayDuration)
        {
            Vibration.Vibrate(vibrationDuration);
            await Task.Delay(delayDuration, CancelToken);
        }

        private async Task SetCancellation()
        {

            
            if(CancelSource != null)
            {
                CancelSource.Cancel();
                
            }

            await Task.Run(async () =>
            {
                int i = 0;
                while (!AllowRun)
                {
                    await Task.Delay(100);
                    i++;
                    if (i > 100)
                    {
                        break;
                    }
                }
            }).ContinueWith((x) => {
                Device.BeginInvokeOnMainThread(() => {
                    CancelSource = null;
                    CancelSource = new CancellationTokenSource();
                    CancelToken = CancelSource.Token;
                });
                
            });

            
            
        }


    }
}
