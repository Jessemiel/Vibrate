using System;
using Android.Content;
using Android.Gms.Ads;
using Android.Widget;
using Vibratr.Controls;
using Vibratr.Droid.Helpers;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdControlView), typeof(AdViewRenderer))]
namespace Vibratr.Droid.Helpers
{
    public class AdViewRenderer:ViewRenderer<AdControlView, AdView>
    {
        public AdViewRenderer(Context context) : base(context) { }

        string adUnitId = "ca-app-pub-5063113731279106/5247957363";
        AdSize adSize = AdSize.Banner;
        AdView adView;

        AdView CreateAdView()
        {
            if (adView != null)
                return adView;

            adView = new AdView(Context)
            {
                AdSize = adSize,
                AdUnitId = adUnitId
            };            

            var adParams = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);

            adView.LayoutParameters = adParams;
            adView.LoadAd(new AdRequest.Builder().Build());

            return adView;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<AdControlView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                CreateAdView();
                SetNativeControl(adView);
            }

        }


    }
}
