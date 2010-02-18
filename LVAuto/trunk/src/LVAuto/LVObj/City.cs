using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
namespace LVAuto.LVObj 
{
    public class City 
	{
        public int Id;
        public string Name;
        public int Size = 0;
        public int X;
        public int Y;
		public int ParentId;
        public LVObj.Building[] AllBuildings;
        public LVObj.MilitaryGeneral[] MilitaryGenerals;
        public LVObj.CivilGeneral[] CivilGenerals;

		public int CountBuilds = 0;
        public ArrayList generalupsikhi;
        public CityTask CityTasks;
		
		public static bool IsBuildAll = false;
		public static bool IsDowndAll = false;

		public static bool IsBuyRes = true;     // buy resources if needed
		public static long GoldSafe = 5000000;  // gold threshold

        public static LVAuto.LVObj.City[] AllCity;

        /// <summary>
        /// Constructs a new City object.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="size"></param>
        public City(int id, string name, int x, int y, int size) 
		{
            this.Id = id;
            this.Name = name;
            this.X = x;
            this.Y = y;
            this.Size = size;
        }

		public City(int id, string name, int x, int y, int size, int parentId)
		{
			this.Id = id;
			this.Name = name;
			this.X = x;
			this.Y = y;
            this.Size = size;
			this.ParentId = parentId;
		}

        public override string ToString() 
		{
            return this.Name;
        }
    } //end class
} //end namespace
