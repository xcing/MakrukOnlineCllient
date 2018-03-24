using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.Component
{
    public class Calculate
    {
        public float getScoreWinAndLose(float score1, float score2, bool isWin)
        {
            float dif;
            float dif1Win;
            float dif1Lose;
            if (score1 > score2)
            {
                dif = Mathf.Round(1000 / (score1 - score2));
                if (dif < 5)
                {
                    dif = 5;
                }
                if (dif > 10)
                {
                    dif = 10;
                }
                dif1Win = dif;

                dif = Mathf.Round((score1 - score2) / 5);
                if (dif < 10)
                {
                    dif = 10;
                }
                if (dif > 80)
                {
                    dif = 80;
                }
                dif1Lose = dif;
            }
            else if (score2 > score1)
            {
                dif = Mathf.Round(1000 / (score2 - score1));
                if (dif < 5)
                {
                    dif = 5;
                }
                if (dif > 10)
                {
                    dif = 10;
                }
                dif1Lose = dif;

                dif = Mathf.Round((score2 - score1) / 5);
                if (dif < 10)
                {
                    dif = 10;
                }
                if (dif > 80)
                {
                    dif = 80;
                }
                dif1Win = dif;
            }
            else
            {
                dif = 10;
                dif1Win = dif;
                dif1Lose = dif;
            }
            if (isWin)
            {
                return dif1Win;
            }
            else
            {
                return dif1Lose;
            }
        }

        public int GetRankFromScore(int score)
        {
            if (score >= 1800)
            {
                return 7;
            }
            else if (score >= 1700)
            {
                return 6;
            }
            else if (score >= 1600)
            {
                return 5;
            }
            else if (score >= 1500)
            {
                return 4;
            }
            else if (score >= 1400)
            {
                return 3;
            }
            else if (score >= 1300)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        public string GetImageFromScore(int score)
        {
            if (score >= 1800)
            {
                return "ico_LV_07";
            }
            else if (score >= 1700)
            {
                return "ico_LV_06";
            }
            else if (score >= 1600)
            {
                return "ico_LV_05";
            }
            else if (score >= 1500)
            {
                return "ico_LV_04";
            }
            else if (score >= 1400)
            {
                return "ico_LV_03";
            }
            else if (score >= 1300)
            {
                return "ico_LV_02";
            }
            else
            {
                return "ico_LV_01";
            }
        }

        public int GetWidthPlus(int amount, int width)
        {
            return width * amount.ToString().Length;
        }
    }
}
