Na Scene Binário tem a Main Camera com Pixel Perfect e a Grid com 2 Tilemaps.

O tilemap platforms tem já as configurações de colisão que se ajeita automaticamente conforme os tiles são posicionados. Basta copiar esses componentes de física e vai funcionar em qualquer um.

O props é um exemplo sem colisores. Por enquanto, não sei como botar prefabs com scripts no tilemap (para botar os vasos e etc), então por enquanto tem que colocar eles fora do tilemap. Estou vendo se funciona identificar tiles pra pegar a posição deles e spawnar coisas no lugar em runtime, deve funcionar