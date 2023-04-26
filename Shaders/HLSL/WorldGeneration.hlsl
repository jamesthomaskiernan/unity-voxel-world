int FindObjAtPos(float3 Pos)
{

	// for (int x = 0; x < AddedObjectsSize; x++)
	// {
	// 	float3 AddedObjPos = AddedObjects[x].Position;
	// 	if (Pos.x == AddedObjPos.x && Pos.y == AddedObjPos.y && Pos.z == AddedObjPos.z)
	// 	{
	// 		return AddedObjects[x].ObjType;
	// 	}
	// }





    if (Pos.y == 1 || Pos.y == 0)
    {
        return 0;
    }

	if (Pos.y == -1)
	{
		return 1;
	}


	float NoiseDefinition = 50.0;

	float noisey = SimplexNoise(Pos / NoiseDefinition);

    noisey -= Pos.y/30.0;



	if (noisey > 0)
	{

		return 1;
		
	}
	else
	{
		return -1;
	}
}









