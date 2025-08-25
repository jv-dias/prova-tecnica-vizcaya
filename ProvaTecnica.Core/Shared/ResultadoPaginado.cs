using System.Collections.Generic;

namespace ProvaTecnica.Core.Shared;

public class ResultadoPaginado<T>
{
	public List<T> Items { get; set; } = new();
	public int TotalCount { get; set; }
}