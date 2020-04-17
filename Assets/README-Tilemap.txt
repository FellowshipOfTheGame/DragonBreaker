Na Scene Binário tem a Main Camera com Pixel Perfect e a Grid com 2 Tilemaps.

O tilemap platforms tem já as configurações de colisão que se ajeita automaticamente conforme os tiles são posicionados. Basta copiar esses componentes de física e vai funcionar em qualquer um.

----------------------------------------------------------------------------------

O props é meio que um manager de tiles de prefab, para usar para posicionar coisas que precisem de script.
Os passos são os seguintes:
1 - cria um novo asset do tipo TileReference, que é um scriptable object;
2 - encontre o TileBase que tem o tile que você quer usar para referenciar o prop em questão (são os .assets criados quando você adiciona um sprite na TilePalette);
3 - referencie o TileBase encontrado e o prefab do prop no TileReference;
4 - finalmente, procure o componente TileSpawner no tilemap dos Props e adicione o novo TileReference na lista

obs.: se quiser pegar a lista de cópias de algum prop, é só usar a função 'GetObjects(NOME_PROP)' da TileSpawner