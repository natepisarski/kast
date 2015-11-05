using System;
using Kast.Base;
using Kast.General;

namespace Kast.General
{
	public interface IBuilder {
		bool Verify(string[] toVerify);
		IKastComponent Build(string[] source);
	}
}

