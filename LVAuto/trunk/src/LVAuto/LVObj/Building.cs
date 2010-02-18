using System;
using System.Collections.Generic;
using System.Text;

namespace LVAuto.LVObj 
{
    public class Building : IComparable 
	{
        //0,2,"Tường thành",29,0,0
        public int PId;
        public int GId;
        public string Name;
        public int Level;
        public int Tab = -99;
		public bool IsUp = false;   //being upgraded
		public bool IsDown = false; //being downgraded

        /// <summary>
        /// Constructs a new Building object.
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="gid"></param>
        /// <param name="name"></param>
        /// <param name="level"></param>
        public Building(int pid, int gid, string name, int level)
		{
            this.PId = pid;
            this.GId = gid;
            this.Name = name;
            this.Level = level;
        }

        public override string ToString() 
		{
            return Name;
        }

        int IComparable.CompareTo(object obj) 
		{
            if (obj is Building) 
			{
                Building temp = (Building)obj;
                return this.Level.CompareTo(temp.Level);
            }
            throw new ArgumentException("Object is not a Building");
        }
    } //end class
} //end namespace
