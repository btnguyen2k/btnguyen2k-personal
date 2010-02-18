using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace LVAuto.LVObj {
    public class CityTask {

        public bool CanBuild = false;
        public bool Canupgrade = false;
        public bool Canmove = false;
        public bool Cantp = false;
        public bool CanMBC = false;

        /// <summary>
        /// Constructs a new CityTask object.
        /// </summary>
        /// <param name="countbuild"></param>
        /// <param name="tech"></param>
        /// <param name="task"></param>
        /// <param name="mcb"></param>
        public CityTask(int countbuild, int tech, int task, int mcb) {
            if (countbuild < 3) {
                CanBuild = true;
            }
            if (tech == 0) {
                Canupgrade = true;
            }
            if (task == 0) {
                Cantp = true;
            }
            if (mcb < 4300 && mcb>0) {
                CanMBC = true;
                System.Media.SoundPlayer mp = new System.Media.SoundPlayer();
                mp.SoundLocation = @"alert.wav";
            }
        }
    } //end class
} //end namespace
