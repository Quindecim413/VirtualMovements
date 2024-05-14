using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Приемник
{
    public class LastCommandAnimationController
    {
        Label labelWithAnimation;
        TimeSpan dotAnimationFrameDuration = TimeSpan.FromSeconds(0.4);
        TimeSpan resultAnimationFrameDuration = TimeSpan.FromSeconds(3);
        public bool LockAnimations = false;
        public bool Visible
        {
            get
            {
                return labelWithAnimation.Visible;
            }
            set
            {
                labelWithAnimation.Visible = value;
            }
        }
        public LastCommandAnimationController(Label label)
        {
            labelWithAnimation = label;
            state = State.ShowDots;
        }
        State state;
        public void ShowSuccess()
        {
            state = State.ShowSuccess;
            lastChange = TimeSpan.Zero;
            
        }
        public void ShowFailure()
        {
            state = State.ShowFailure;
            lastChange = TimeSpan.Zero;
        }
        public void ShowInProgress()
        {
            state = State.ShowDots;
            lastChange = TimeSpan.Zero;
        }
        TimeSpan lastChange = TimeSpan.Zero;
        int dotsAmount = 1;
        public void Update(TimeSpan timePassed)
        {
            switch (state)
            {
                case State.ShowDots:
                    if((timePassed + lastChange).TotalSeconds > dotAnimationFrameDuration.TotalSeconds)
                    {
                        lastChange = TimeSpan.Zero;
                        dotsAmount = (dotsAmount % 4);
                        dotsAmount++;
                    }
                    lastChange = lastChange + timePassed;
                    labelWithAnimation.Text = "";
                    for (int i = 0; i < dotsAmount; i++)
                        labelWithAnimation.Text += ".";

                    labelWithAnimation.ForeColor = Color.Blue;
                    break;
                case State.ShowFailure:
                    if(lastChange + timePassed > resultAnimationFrameDuration)
                    {
                        state = State.ShowDots;
                        Update(timePassed);
                        return;
                    }
                    lastChange = lastChange + timePassed;
                    labelWithAnimation.Text = "-";
                    labelWithAnimation.ForeColor = Color.Red;
                    dotsAmount = 1;
                    break;
                case State.ShowSuccess:
                    if (lastChange + timePassed > resultAnimationFrameDuration)
                    {
                        state = State.ShowDots;
                        Update(timePassed);
                        return;
                    }
                    lastChange = lastChange + timePassed;
                    labelWithAnimation.Text = "+";
                    labelWithAnimation.ForeColor = Color.Green;
                    dotsAmount = 1;
                    break;
            }
        }

        private enum State
        {
            ShowSuccess,
            ShowFailure,
            ShowDots
        }
    }
}
