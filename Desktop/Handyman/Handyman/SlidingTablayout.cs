using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Handyman
{
  public class SlidingTablayout : HorizontalScrollView
    {
        private const int TITLE_OFFSET_DIPS = 24;
        private const int TAB_VIEW_PADDING_DIPS = 16;
        private const int TAB_VIEW_TEXT_SIZE = 12;

        private int mtitleOffset;

        private ViewPager mViewPager;
        private ViewPager.IOnPageChangeListener mViewPagerChangeListener;

        private static slidingActivities mTabStrip;
        private int mScrollState;
        
        public interface TabColorizer
        {
            int GetIndicatorColors(int position);
            int GetDividedColors(int position);
        }

        public SlidingTablayout(Context context) :  this(context, null) { }
        
        public SlidingTablayout(Context context,IAttributeSet attrs): this(context, attrs, 0) { }

        public SlidingTablayout(Context context, IAttributeSet attrs,int defaultstyle): base(context,attrs,defaultstyle)
        {
            //Disable scroll view
            HorizontalScrollBarEnabled = false;

            // check if strips fill the view
            FillViewport = true;
            this.SetBackgroundColor(Android.Graphics.Color.Rgb(0xE5, 0xE5, 0xE5));

            mtitleOffset = (int)(TITLE_OFFSET_DIPS * Resources.DisplayMetrics.Density);
            mTabStrip = new slidingActivities(context);
            this.AddView(mTabStrip, LayoutParams.MatchParent, LayoutParams.MatchParent);
        }
        public TabColorizer customTabColorizer
        {
            set { mTabStrip.customTabColorizer = value;
                this.Invalidate();
            }
        }

        public int [] selectedIndicatorColors
        {
            set
            {
              //  mTabStrip
            }
        }
    }
}