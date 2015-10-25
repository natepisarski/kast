using System;

using System.Collections.Generic;

namespace Kast.General
{
	public interface IKastComponent
	{
		void PulseReact();
		string Latest();
	}
}

