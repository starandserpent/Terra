using System.Linq;
public class Mesher {
    public static MeshedValues NaiveGreedyMeshing (Chunk chunk) {
        MeshedValues values = new MeshedValues ();
        int[][][, , , ] vertices = new int[chunk.materials - 1][][, , , ];
        int count = 0;
        int prev = 0;
        int[] indice = new int[chunk.materials - 1];
        int objectID = 0;
        int lenght;

        for (int i = 1; count < Constants.CHUNK_SIZE3D; i++) {

            if (prev > 0) {
                i--;
                lenght = prev;
                prev = 0;
            } else {
                Run run = chunk.voxels[chunk.voxels.Count () - i];
                objectID = run.value;
                lenght = run.lenght;
            }

            if (objectID == 0) {
                count += lenght;
                continue;
            }

            if (vertices[objectID - 1] == null) {
                vertices[objectID - 1] = new int[6][, , , ];
                for (int s = 0; s < 6; s++) {
                    vertices[objectID - 1][s] = new int[Constants.CHUNK_SIZE1D, Constants.CHUNK_SIZE1D, 4, 3];
                }
            }

            int z = count / Constants.CHUNK_SIZE2D;
            int y = count % Constants.CHUNK_SIZE1D;
            int x = (count / Constants.CHUNK_SIZE1D) % Constants.CHUNK_SIZE1D;

            if (lenght / Constants.CHUNK_SIZE1D > 0) {
                int size = Constants.CHUNK_SIZE1D - y;
                prev = lenght - size;
                lenght = size;
            }

            int ax = x + 1;
            int ay = lenght + y;
            int az = z + 1;

            //Front
            int[, , , ] vectors = vertices[objectID - 1][0];

            //1
            vectors[x, z, 0, 0] = x;
            vectors[x, z, 0, 1] = y;
            vectors[x, z, 0, 2] = z;

            //2
            vectors[x, z, 1, 0] = ax;
            vectors[x, z, 1, 1] = y;
            vectors[x, z, 1, 2] = z;

            //3
            vectors[x, z, 2, 0] = ax;
            vectors[x, z, 2, 1] = ay;
            vectors[x, z, 2, 2] = z;

            //4
            vectors[x, z, 3, 0] = x;
            vectors[x, z, 3, 1] = ay;
            vectors[x, z, 3, 2] = z;

            int pos = 1;
            if (z > 0 && vectors[x, z - 1, 2, 1] > 0) {

                if (vectors[x, z - pos, 0, 0] < 0) {
                    pos = (int) - vectors[x, z - pos, 0, 0];
                }

                if (vectors[x, z - pos, 2, 1] >= ay) {
                    for (int s = 0; s < 4; s++) {
                        vectors[x, z, s, 0] = -pos;
                    }
                    indice[objectID - 1] -= 4;
                } else if (vectors[x, z - pos, 2, 1] < ay && vectors[x, z - pos, 0, 1] >= y) {
                    vectors[x, z, 0, 1] = vectors[x, z - pos, 2, 1];
                    vectors[x, z, 1, 1] = vectors[x, z - pos, 2, 1];
                }
            }

            //Greedy Meshing
         /*   if (z > 0 && vectors[x, z - 1, 0, 1] == ay && vectors[x, z - 1, 0, 0] > 0 && vectors[x, z - 1, 0, 0] == sx && vectors[x, z - 1, 1, 0] == sx) {
                vectors[x, z - 1, 0, 2] = vectors[x, z - 1, 0, 2];
                for (int s = 0; s < 4; s++) {
                    vectors[x, z - 1, s, 0] = -147457;
                }

                indice[objectID - 1] -= 4;
            }

            indice[objectID - 1] += 4;*/

            //Back
            /*  vectors = vertices[objectID - 1][1];

              //1
              vectors[x, z, 0, 0] = ax;
              vectors[x, z, 0, 1] = y;
              vectors[x, z, 0, 2] = az;

              //2
              vectors[x, z, 1, 0] = x;
              vectors[x, z, 1, 1] = y;
              vectors[x, z, 1, 2] = az;

              //3
              vectors[x, z, 2, 0] = x;
              vectors[x, z, 2, 1] = ay;
              vectors[x, z, 2, 2] = az;

              //5
              vectors[x, z, 3, 0] = ax;
              vectors[x, z, 3, 1] = ay;
              vectors[x, z, 3, 2] = az;

              if (z > 0 && vectors[x, z - 1, 2, 1] > 0) {
                  if (vectors[x, z - 1, 2, 1] > ay && vectors[x, z - 1, 0, 1] <= y) {
                      vectors[x, z - 1, 0, 1] = ay;
                      vectors[x, z - 1, 1, 1] = ay;
                  } else if (vectors[x, z - 1, 2, 1] <= ay) {
                      for (int s = 0; s < 4; s++) {
                          vectors[x, z - 1, s, 0] = -147457;
                      }

                      indice[objectID - 1] -= 4;
                  }
              }

              indice[objectID - 1] += 4;

              //Right
              vectors = vertices[objectID - 1][2];

              //1
              vectors[x, z, 0, 0] = ax;
              vectors[x, z, 0, 1] = y;
              vectors[x, z, 0, 2] = z;

              //2
              vectors[x, z, 1, 0] = ax;
              vectors[x, z, 1, 1] = y;
              vectors[x, z, 1, 2] = az;

              //3
              vectors[x, z, 2, 0] = ax;
              vectors[x, z, 2, 1] = ay;
              vectors[x, z, 2, 2] = az;

              //4
              vectors[x, z, 3, 0] = ax;
              vectors[x, z, 3, 1] = ay;
              vectors[x, z, 3, 2] = z;

              if (x > 0 && vectors[x - 1, z, 2, 1] > 0) {
                  if (vectors[x - 1, z, 2, 1] > ay && vectors[x - 1, z, 0, 1] <= y) {
                      vectors[x - 1, z, 0, 1] = ay;
                      vectors[x - 1, z, 1, 1] = ay;
                  } else if (vectors[x - 1, z, 2, 1] <= ay) {
                      for (int s = 0; s < 4; s++) {
                          vectors[x - 1, z, s, 0] = -147457;
                      }

                      indice[objectID - 1] -= 4;
                  }
              }

              indice[objectID - 1] += 4;

              //Left
              vectors = vertices[objectID - 1][3];

              //1
              vectors[x, z, 0, 0] = x;
              vectors[x, z, 0, 1] = y;
              vectors[x, z, 0, 2] = az;

              //2
              vectors[x, z, 1, 0] = x;
              vectors[x, z, 1, 1] = y;
              vectors[x, z, 1, 2] = z;

              //3
              vectors[x, z, 2, 0] = x;
              vectors[x, z, 2, 1] = ay;
              vectors[x, z, 2, 2] = z;

              //4
              vectors[x, z, 3, 0] = x;
              vectors[x, z, 3, 1] = ay;
              vectors[x, z, 3, 2] = az;

              pos = 1;
              if (x > 0 && vectors[x - 1, z, 2, 1] > 0) {
                  if (vectors[x - pos, z, 0, 0] < 0) {
                      pos = (int) - vectors[x - pos, z, 0, 0];
                  }

                  if (vectors[x - pos, z, 2, 1] >= ay) {
                      for (int s = 0; s < 4; s++) {
                          vectors[x, z, s, 0] = -pos;
                      }

                      indice[objectID - 1] -= 4;
                  } else if (vectors[x - pos, z, 2, 1] < ay && vectors[x - pos, z, 0, 1] >= y) {
                      vectors[x, z, 0, 1] = vectors[x - pos, z, 2, 1];
                      vectors[x, z, 1, 1] = vectors[x - pos, z, 2, 1];
                  }
              }

              indice[objectID - 1] += 4;

              //Top
              vectors = vertices[objectID - 1][4];

              int sx = x;
              if (x > 0 && vectors[x - 1, z, 0, 1] == ay && vectors[x - 1, z, 1, 0] > 0) {
                  sx = vectors[x - 1, z, 0, 0];
                  for (int s = 0; s < 4; s++) {
                      vectors[x - 1, z, s, 0] = -147457;
                  }

                  indice[objectID - 1] -= 4;
              }

              //1
              vectors[x, z, 0, 0] = sx;
              vectors[x, z, 0, 1] = ay;
              vectors[x, z, 0, 2] = z;

              //2
              vectors[x, z, 1, 0] = ax;
              vectors[x, z, 1, 1] = ay;
              vectors[x, z, 1, 2] = z;

              //3
              vectors[x, z, 2, 0] = ax;
              vectors[x, z, 2, 1] = ay;
              vectors[x, z, 2, 2] = az;

              //5
              vectors[x, z, 3, 0] = sx;
              vectors[x, z, 3, 1] = ay;
              vectors[x, z, 3, 2] = az;

              indice[objectID - 1] += 4;

              //Bottom
              vectors = vertices[objectID - 1][5];

              sx = x;
              if (x > 0 && vectors[x - 1, z, 0, 1] == y && vectors[x - 1, z, 0, 0] > 0) {
                  sx = vectors[x - 1, z, 1, 0];
                  for (int s = 0; s < 4; s++) {
                      vectors[x - 1, z, s, 0] = -147457;
                  }

                  indice[objectID - 1] -= 4;
              }

              //1
              vectors[x, z, 0, 0] = ax;
              vectors[x, z, 0, 1] = y;
              vectors[x, z, 0, 2] = z;

              //2
              vectors[x, z, 1, 0] = sx;
              vectors[x, z, 1, 1] = y;
              vectors[x, z, 1, 2] = z;

              //3
              vectors[x, z, 2, 0] = sx;
              vectors[x, z, 2, 1] = y;
              vectors[x, z, 2, 2] = az;

              //4
              vectors[x, z, 3, 0] = ax;
              vectors[x, z, 3, 1] = y;
              vectors[x, z, 3, 2] = az;

              indice[objectID - 1] += 4;*/

            count += lenght;
        }

        values.vertices = vertices;
        values.indices = indice;
        return values;
    }

    public static MeshedValues GreedyMeshing (Chunk chunk) {
        MeshedValues values = new MeshedValues ();
        int[][][, , , ] vertices = new int[chunk.materials - 1][][, , , ];
        int count = 0;
        int prev = 0;
        int[] indice = new int[chunk.materials - 1];
        int objectID = 0;
        int lenght;

        for (int i = 1; count < Constants.CHUNK_SIZE3D; i++) {

            if (prev > 0) {
                i--;
                lenght = prev;
                prev = 0;
            } else {
                Run run = chunk.voxels[chunk.voxels.Count () - i];
                objectID = run.value;
                lenght = run.lenght;
            }

            if (objectID == 0) {
                count += lenght;
                continue;
            }

            if (vertices[objectID - 1] == null) {
                vertices[objectID - 1] = new int[6][, , , ];
                for (int s = 0; s < 6; s++) {
                    vertices[objectID - 1][s] = new int[Constants.CHUNK_SIZE1D, Constants.CHUNK_SIZE1D, 4, 3];
                }
            }

            int z = count / Constants.CHUNK_SIZE2D;
            int y = count % Constants.CHUNK_SIZE1D;
            int x = (count / Constants.CHUNK_SIZE1D) % Constants.CHUNK_SIZE1D;

            if (lenght / Constants.CHUNK_SIZE1D > 0) {
                int size = Constants.CHUNK_SIZE1D - y;
                prev = lenght - size;
                lenght = size;
            }

            int ax = x + 1;
            int ay = lenght + y;
            int az = z + 1;

            //Front
            int[, , , ] vectors = vertices[objectID - 1][0];

            //1
            vectors[x, z, 0, 0] = x;
            vectors[x, z, 0, 1] = y;
            vectors[x, z, 0, 2] = z;

            //2
            vectors[x, z, 1, 0] = ax;
            vectors[x, z, 1, 1] = y;
            vectors[x, z, 1, 2] = z;

            //3
            vectors[x, z, 2, 0] = ax;
            vectors[x, z, 2, 1] = ay;
            vectors[x, z, 2, 2] = z;

            //4
            vectors[x, z, 3, 0] = x;
            vectors[x, z, 3, 1] = ay;
            vectors[x, z, 3, 2] = z;

            //Naive Greedy Meshing
            int pos = 1;
            if (z > 0 && vectors[x, z - 1, 2, 1] > 0) {

                if (vectors[x, z - pos, 0, 0] < 0) {
                    pos = (int) - vectors[x, z - pos, 0, 0];
                }

                if (vectors[x, z - pos, 2, 1] >= ay) {
                    for (int s = 0; s < 4; s++) {
                        vectors[x, z, s, 0] = -pos;
                    }
                    indice[objectID - 1] -= 4;
                } else if (vectors[x, z - pos, 2, 1] < ay && vectors[x, z - pos, 0, 1] >= y) {
                    vectors[x, z, 0, 1] = vectors[x, z - pos, 2, 1];
                    vectors[x, z, 1, 1] = vectors[x, z - pos, 2, 1];
                }
            }

            indice[objectID - 1] += 4;

            //Back
            vectors = vertices[objectID - 1][1];

            //1
            vectors[x, z, 0, 0] = ax;
            vectors[x, z, 0, 1] = y;
            vectors[x, z, 0, 2] = az;

            //2
            vectors[x, z, 1, 0] = x;
            vectors[x, z, 1, 1] = y;
            vectors[x, z, 1, 2] = az;

            //3
            vectors[x, z, 2, 0] = x;
            vectors[x, z, 2, 1] = ay;
            vectors[x, z, 2, 2] = az;

            //5
            vectors[x, z, 3, 0] = ax;
            vectors[x, z, 3, 1] = ay;
            vectors[x, z, 3, 2] = az;

            if (z > 0 && vectors[x, z - 1, 2, 1] > 0) {
                if (vectors[x, z - 1, 2, 1] > ay && vectors[x, z - 1, 0, 1] <= y) {
                    vectors[x, z - 1, 0, 1] = ay;
                    vectors[x, z - 1, 1, 1] = ay;
                } else if (vectors[x, z - 1, 2, 1] <= ay) {
                    for (int s = 0; s < 4; s++) {
                        vectors[x, z - 1, s, 0] = -147457;
                    }

                    indice[objectID - 1] -= 4;
                }
            }

            indice[objectID - 1] += 4;

            //Right
            vectors = vertices[objectID - 1][2];

            //1
            vectors[x, z, 0, 0] = ax;
            vectors[x, z, 0, 1] = y;
            vectors[x, z, 0, 2] = z;

            //2
            vectors[x, z, 1, 0] = ax;
            vectors[x, z, 1, 1] = y;
            vectors[x, z, 1, 2] = az;

            //3
            vectors[x, z, 2, 0] = ax;
            vectors[x, z, 2, 1] = ay;
            vectors[x, z, 2, 2] = az;

            //4
            vectors[x, z, 3, 0] = ax;
            vectors[x, z, 3, 1] = ay;
            vectors[x, z, 3, 2] = z;

            //Naive Greedy Meshing
            if (x > 0 && vectors[x - 1, z, 2, 1] > 0) {
                if (vectors[x - 1, z, 2, 1] > ay && vectors[x - 1, z, 0, 1] <= y) {
                    vectors[x - 1, z, 0, 1] = ay;
                    vectors[x - 1, z, 1, 1] = ay;
                } else if (vectors[x - 1, z, 2, 1] <= ay) {
                    for (int s = 0; s < 4; s++) {
                        vectors[x - 1, z, s, 0] = -147457;
                    }

                    indice[objectID - 1] -= 4;
                }
            }

            indice[objectID - 1] += 4;

            //Left
            vectors = vertices[objectID - 1][3];

            //1
            vectors[x, z, 0, 0] = x;
            vectors[x, z, 0, 1] = y;
            vectors[x, z, 0, 2] = az;

            //2
            vectors[x, z, 1, 0] = x;
            vectors[x, z, 1, 1] = y;
            vectors[x, z, 1, 2] = z;

            //3
            vectors[x, z, 2, 0] = x;
            vectors[x, z, 2, 1] = ay;
            vectors[x, z, 2, 2] = z;

            //4
            vectors[x, z, 3, 0] = x;
            vectors[x, z, 3, 1] = ay;
            vectors[x, z, 3, 2] = az;

            //Naive Greedy Meshing
            pos = 1;
            if (x > 0 && vectors[x - 1, z, 2, 1] > 0) {
                if (vectors[x - pos, z, 0, 0] < 0) {
                    pos = (int) - vectors[x - pos, z, 0, 0];
                }

                if (vectors[x - pos, z, 2, 1] >= ay) {
                    for (int s = 0; s < 4; s++) {
                        vectors[x, z, s, 0] = -pos;
                    }

                    indice[objectID - 1] -= 4;
                } else if (vectors[x - pos, z, 2, 1] < ay && vectors[x - pos, z, 0, 1] >= y) {
                    vectors[x, z, 0, 1] = vectors[x - pos, z, 2, 1];
                    vectors[x, z, 1, 1] = vectors[x - pos, z, 2, 1];
                }
            }

            indice[objectID - 1] += 4;

            //Top
            vectors = vertices[objectID - 1][4];

            //Naive Greedy Meshing
            int sx = x;
            int sz = z;
            if (x > 0 && vectors[x - 1, z, 0, 1] == ay && vectors[x - 1, z, 1, 0] > 0) {
                sx = vectors[x - 1, z, 0, 0];
                for (int s = 0; s < 4; s++) {
                    vectors[x - 1, z, s, 0] = -147457;
                }

                indice[objectID - 1] -= 4;
            }

            if (z > 0 && vectors[x, z - 1, 0, 1] == ay && vectors[x, z - 1, 0, 0] > 0 && vectors[x, z - 1, 0, 0] == sx && vectors[x, z - 1, 1, 0] == sx) {
                sz = vectors[x, z - 1, 0, 2];
                for (int s = 0; s < 4; s++) {
                    vectors[x, z - 1, s, 0] = -147457;
                }

                indice[objectID - 1] -= 4;
            }

            //1
            vectors[x, z, 0, 0] = sx;
            vectors[x, z, 0, 1] = ay;
            vectors[x, z, 0, 2] = z;

            //2
            vectors[x, z, 1, 0] = ax;
            vectors[x, z, 1, 1] = ay;
            vectors[x, z, 1, 2] = z;

            //3
            vectors[x, z, 2, 0] = ax;
            vectors[x, z, 2, 1] = ay;
            vectors[x, z, 2, 2] = az;

            //5
            vectors[x, z, 3, 0] = sx;
            vectors[x, z, 3, 1] = ay;
            vectors[x, z, 3, 2] = az;

            indice[objectID - 1] += 4;

            //Bottom
            vectors = vertices[objectID - 1][5];

            //Naive Greedy Meshing
            sx = x;
            sz = z;
            if (x > 0 && vectors[x - 1, z, 0, 1] == y && vectors[x - 1, z, 0, 0] > 0) {
                sx = vectors[x - 1, z, 1, 0];
                for (int s = 0; s < 4; s++) {
                    vectors[x - 1, z, s, 0] = -147457;
                }

                indice[objectID - 1] -= 4;
            }

            //Greedy Meshing
            if (z > 0 && vectors[x, z - 1, 0, 1] == y && vectors[x, z - 1, 0, 0] > 0 && vectors[x, z - 1, 0, 0] == ax && vectors[x, z - 1, 2, 0] == sx) {
                sz = vectors[x, z - 1, 0, 2];
                for (int s = 0; s < 4; s++) {
                    vectors[x, z - 1, s, 0] = -147457;
                }

                indice[objectID - 1] -= 4;
            }

            //1
            vectors[x, z, 0, 0] = ax;
            vectors[x, z, 0, 1] = y;
            vectors[x, z, 0, 2] = sz;

            //2
            vectors[x, z, 1, 0] = sx;
            vectors[x, z, 1, 1] = y;
            vectors[x, z, 1, 2] = sz;

            //3
            vectors[x, z, 2, 0] = sx;
            vectors[x, z, 2, 1] = y;
            vectors[x, z, 2, 2] = az;

            //4
            vectors[x, z, 3, 0] = ax;
            vectors[x, z, 3, 1] = y;
            vectors[x, z, 3, 2] = az;

            indice[objectID - 1] += 4;

            count += lenght;
        }

        values.vertices = vertices;
        values.indices = indice;
        return values;
    }
}