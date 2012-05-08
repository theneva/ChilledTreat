using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChilledTreat.Tools
{
	class Trapeze
	{
		static Trapeze _instance;
		public static Trapeze Instance
		{
			get { return _instance ?? (_instance = new Trapeze()); }
		}

		private Point _topLeft, _topRight, _bottomLeft, _bottomRight;
		private Vector2 _leftSide, _rightSide, _top, _bottom;

		private Trapeze()
		{
			_topLeft = new Point(537, 510);
			_topRight = new Point(730, 510);
			_bottomLeft = new Point(111, 0);
			_bottomRight = new Point(1196, 0);
		}

		public bool IsInside(Point p)
		{

			return false;
		}
	}
}
