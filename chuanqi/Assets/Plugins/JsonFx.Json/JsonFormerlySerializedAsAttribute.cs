using System;
namespace Pathfinding.Serialization.JsonFx
{
	/** Indicate that this was previously serialized as [name] and can load values from fields with that name in a json file */
	public class JsonFormerlySerializedAsAttribute : Attribute
	{
		public string name;
		public JsonFormerlySerializedAsAttribute (string name)
		{
			this.name = name;
		}
	}
}

