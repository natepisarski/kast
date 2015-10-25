using System;

namespace Kast
{
	public interface IBuilder {
		bool Verify(string[] toVerify);
		Kast.KastComponent Build(string[] source);
	}
}

