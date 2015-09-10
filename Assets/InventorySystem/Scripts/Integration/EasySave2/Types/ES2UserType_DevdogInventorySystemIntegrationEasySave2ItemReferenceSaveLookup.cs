#if EASY_SAVE_2

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Devdog.InventorySystem.Integration.EasySave2;

public class ES2UserType_DevdogInventorySystemIntegrationEasySave2ItemReferenceSaveLookup : ES2Type
{
	public override void Write(object obj, ES2Writer writer)
	{
		Devdog.InventorySystem.Integration.EasySave2.ItemReferenceSaveLookup data = (Devdog.InventorySystem.Integration.EasySave2.ItemReferenceSaveLookup)obj;
		// Add your writer.Write calls here.
		writer.Write(data.referenceOfCollection);
		writer.Write(data.itemID);
		writer.Write(data.amount);

	}
	
	public override object Read(ES2Reader reader)
	{
		Devdog.InventorySystem.Integration.EasySave2.ItemReferenceSaveLookup data = new Devdog.InventorySystem.Integration.EasySave2.ItemReferenceSaveLookup();
		// Add your reader.Read calls here and return your object.
		data.referenceOfCollection = reader.Read<System.String>();
		data.itemID = reader.Read<System.Int32>();
		data.amount = reader.Read<System.UInt32>();

		return data;
	}
	
	/* ! Don't modify anything below this line ! */
	public ES2UserType_DevdogInventorySystemIntegrationEasySave2ItemReferenceSaveLookup():base(typeof(Devdog.InventorySystem.Integration.EasySave2.ItemReferenceSaveLookup)){}
}

#endif