using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Util;

namespace Handyman
{
   public class slidingActivities : LinearLayout
    {
        private const int DEFAULT_BOTTOM_BORDER_THICKNESS = 2;
        private const byte DEFAULT_BORDER_COLOR = 0X26;
        private const int SELECTED_INDICATOR_THICKNESS = 8;
        private int[] INDICATOR_COLOR = { 0x19A319, 0x0000FC };
        private int[] DIVIDER_COLORS = { 0xC5C5C5 };


        private const int DEFAULT_DIVIDER_THICNESS = 1;
        private const float DEFAULT_HEIGHT_DIVIDER = 0.5f;
        //Bottom bordeder of the tab
        private int mBottomOrderThickness;
        private Paint mBottomBorderPaint;
        private int mDefaultBottomColor;
        //Indicator style components 
        private int mSelectedIndicatorThickness;
        private Paint mSelectedIndicatorPaint;

        //Divider
        private Paint mDividerPaint;
        private float mDividerHeigth;

        //selected position and offest
        private int mSelectedPosition;
        private float mSelectedonOffset;
        //Tab color
        private SlidingTablayout.TabColorizer mCustomTabColorizer;
        private SimpleTabColorizer mDefaultColorizer;
        private object p;

        //constructors
        public slidingActivities(Context context) : this(context, null)
        {

        }

        public slidingActivities(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            SetWillNotDraw(false);
            float density = Resources.DisplayMetrics.Density;
            TypedValue outvalue = new TypedValue();
            context.Theme.ResolveAttribute(Android.Resource.Attribute.ColorForeground, outvalue, true);
            int themeForground = outvalue.Data;
            mDefaultBottomColor = setColorAlpha(themeForground, DEFAULT_BORDER_COLOR);

            mDefaultColorizer = new SimpleTabColorizer();
            mDefaultColorizer.Indicators=INDICATOR_COLOR;
            mDefaultColorizer.DividerColors=DIVIDER_COLORS;

            mBottomOrderThickness = (int)(DEFAULT_BOTTOM_BORDER_THICKNESS * density);
            mBottomBorderPaint = new Paint();
            mBottomBorderPaint.Color = GetColorFromInteger(0xC5C5C5);

            mSelectedIndicatorThickness = (int)(SELECTED_INDICATOR_THICKNESS * density);
            mSelectedIndicatorPaint = new Paint();

            mDividerHeigth = DEFAULT_HEIGHT_DIVIDER;
            mDividerPaint = new Paint();
            mDividerPaint.StrokeWidth = (int)(DEFAULT_DIVIDER_THICNESS * density);

        }

        public SlidingTablayout.TabColorizer customTabColorizer
        {
            set { mCustomTabColorizer = value;
                this.Invalidate();
            }
        }

        public int[] selectedIndicatorColors
        {
            set { mCustomTabColorizer = null;
                mDefaultColorizer.Indicators = value;
                    this.Invalidate(); }
        }

        public int[] DividerColors
        {
            set
            {
                mDefaultColorizer = null;
                mDefaultColorizer.DividerColors = value;
                this.Invalidate();
            }
        }
        private Color GetColorFromInteger(int color)
        {
            return Color.Rgb(Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }

        private int setColorAlpha(int color, byte alpha)
        {
            //throw new NotImplementedException();
            return Color.Argb(alpha, Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }

        private int SetColorAlpha(int color,byte alpha)
        {
            return Color.Argb(alpha, Color.GetRedComponent(color), Color.GetGreenComponent(color), Color.GetBlueComponent(color));
        }

        public void OnviewPageChanged(int position,float positionOffset)
        {
            mSelectedPosition = position;
            mSelectedonOffset = positionOffset;
            this.Invalidate();
        }

        public override void OnDrawForeground(Canvas canvas)
        {
            int height = Height;
            int childCount = ChildCount;
            int dividerHeightPx  = (int)(Math.Min(Math.Max(0f,mDividerHeigth), 1f ) * height);
            SlidingTablayout.TabColorizer tabcolorizer = mCustomTabColorizer  != null ? mCustomTabColorizer : mDefaultColorizer;


            if(childCount > 0)
            {
                View selectedTitle = GetChildAt(mSelectedPosition);
                int left = selectedTitle.Left;
                int right = selectedTitle.Right;
                int color = tabcolorizer.GetIndicatorColors(mSelectedPosition);

                if(mSelectedonOffset > 0f && mSelectedPosition < (childCount -1))
                {
                    int nextColor = tabcolorizer.GetIndicatorColors(mSelectedPosition + 1);
                    if(color != nextColor)
                    {
                        color = blendColor(nextColor, color, mSelectedonOffset);
                    }

                    View nextTile = GetChildAt(mSelectedPosition + 1);
                    left = (int)(mSelectedonOffset * nextTile.Left + (1.0f - mSelectedonOffset) * left);
                    right = (int)(mSelectedonOffset * nextTile.Right + (1.0f - mSelectedonOffset) * right);
                }

                mSelectedIndicatorPaint.Color = GetColorFromInteger(color);
                canvas.DrawRect(left, height - mSelectedIndicatorThickness, right, height, mSelectedIndicatorPaint);

                int seperatorTop = (height - dividerHeightPx) / 2;
                for (int i=0; i<childCount;i++)
                {
                    View child = GetChildAt(i);
                    mDividerPaint.Color = GetColorFromInteger(tabcolorizer.GetDividedColors(i));
                    canvas.DrawLine(child.Right, seperatorTop, child.Right, seperatorTop + dividerHeightPx,mDividerPaint);
                }

                canvas.DrawRect(0, height - mBottomOrderThickness,Width,height,mBottomBorderPaint);
            }
        }

        private int blendColor(int nextColor, int color, float ratio)
        {
            float inversRatio = 1f - ratio;
            float r = (Color.GetRedComponent(color) * ratio) + (Color.GetRedComponent(nextColor) * inversRatio);
            float g = (Color.GetGreenComponent(color) * ratio) + (Color.GetGreenComponent(nextColor) * inversRatio);
            float b = (Color.GetBlueComponent(color) * ratio) + (Color.GetBlueComponent(nextColor) * inversRatio);

            return Color.Rgb((int)r, (int)g, (int)b);

        }

        public class SimpleTabColorizer : SlidingTablayout.TabColorizer
        {
            private int[] mIndicatorColors;
            private int[] mDividercolors;

            public int GetIndicatorColors(int position)
            {
                return mIndicatorColors[position % mIndicatorColors.Length];
            }

            public int GetDividedColors(int position)
            {
                return mDividercolors[position % mDividercolors.Length];
            }

            public int [] Indicators
            {
                set { mIndicatorColors = value; }
            }

            public int [] DividerColors
            {
                set { mDividercolors = value; }
            }
        }
    }
}