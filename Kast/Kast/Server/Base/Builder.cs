using System;
using Kast.Server.Base;
using Kast.Server.General;

namespace Kast.Server.Base
{
	public interface IBuilder {
		bool Verify(string[] toVerify);
		IKastComponent Build(string[] source);
	}
}

