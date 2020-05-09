using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class Mesher {
    public static MeshedValues NaiveGreedyMeshing (Chunk chunk) {
        MeshedValues values = new MeshedValues ();
        Position[][][][] vertices = new Position[chunk.materials - 1][][][];
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
                vertices[objectID - 1] = new Position[6][][];
                for (int s = 0; s < 6; s++) {
                    vertices[objectID - 1][s] = ArrayPool<Position[]>.Shared.Rent (Constants.CHUNK_SIZE1D * Constants.CHUNK_SIZE1D);
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

            Position[][] vectors = vertices[objectID - 1][0];

            //Front
            vectors[x + (z * Constants.CHUNK_SIZE1D)] = new Position[4];

            //1
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].x = x;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].y = y;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].z = z;

            //2
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].x = ax;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].y = y;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].z = z;

            //3
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].x = ax;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].y = ay;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].z = z;

            //4
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].x = x;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].y = ay;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].z = z;

            if (z > 0 && vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D] != null) {
                if (vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D][2].y >= ay) {
                    for (int s = 0; s < 4; s++) {
                        vectors[x + (z * Constants.CHUNK_SIZE1D)][s].delete = true;
                    }
                } else if (vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D][2].y < ay && vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D][0].y >= y) {
                    vectors[x + (z * Constants.CHUNK_SIZE1D)][0].y = vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D][2].y;
                    vectors[x + (z * Constants.CHUNK_SIZE1D)][1].y = vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D][2].y;
                }
            }

            indice[objectID - 1] += 4;

            //Back
            vectors = vertices[objectID - 1][1];
            vectors[x + (z * Constants.CHUNK_SIZE1D)] = new Position[4];

            //1
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].x = ax;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].y = y;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].z = az;

            //2
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].x = x;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].y = y;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].z = az;

            //3
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].x = x;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].y = ay;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].z = az;

            //5
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].x = ax;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].y = ay;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].z = az;

            if (z > 0 && vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D] != null) {
                if (vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D][2].y > ay && vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D][0].y <= y) {
                    vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D][0].y = ay;
                    vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D][1].y = ay;
                } else if (vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D][2].y <= ay) {
                    vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D] = null;
                    indice[objectID - 1] -= 4;
                }
            }

            indice[objectID - 1] += 4;

            //Right
            vectors = vertices[objectID - 1][2];
            vectors[x + (z * Constants.CHUNK_SIZE1D)] = new Position[4];

            //1
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].x = ax;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].y = y;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].z = z;

            //2
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].x = ax;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].y = y;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].z = az;

            //3
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].x = ax;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].y = ay;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].z = az;

            //4
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].x = ax;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].y = ay;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].z = z;

            if (x > 0 && vectors[x + (z * Constants.CHUNK_SIZE1D) - 1] != null) {
                if (vectors[x + (z * Constants.CHUNK_SIZE1D) - 1][2].y > ay && vectors[x + (z * Constants.CHUNK_SIZE1D) - 1][0].y <= y) {
                    vectors[x + (z * Constants.CHUNK_SIZE1D) - 1][0].y = ay;
                    vectors[x + (z * Constants.CHUNK_SIZE1D) - 1][1].y = ay;
                } else if (vectors[x + (z * Constants.CHUNK_SIZE1D) - 1][2].y <= ay) {
                    vectors[x + (z * Constants.CHUNK_SIZE1D) - 1] = null;
                    indice[objectID - 1] -= 4;
                }
            }

            indice[objectID - 1] += 4;

            //Left
            vectors = vertices[objectID - 1][3];
            vectors[x + (z * Constants.CHUNK_SIZE1D)] = new Position[4];

            //1
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].x = x;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].y = y;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].z = az;

            //2
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].x = x;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].y = y;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].z = z;

            //3
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].x = x;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].y = ay;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].z = z;

            //4
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].x = x;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].y = ay;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].z = az;

            if (x > 0 && vectors[x + (z * Constants.CHUNK_SIZE1D) - 1] != null) {
                if (vectors[x + (z * Constants.CHUNK_SIZE1D) - 1][2].y >= ay) {
                    for (int s = 0; s < 4; s++) {
                        vectors[x + (z * Constants.CHUNK_SIZE1D)][s].delete = true;
                    }
                } else if (vectors[x + (z * Constants.CHUNK_SIZE1D) - 1][2].y < ay && vectors[x + (z * Constants.CHUNK_SIZE1D) - 1][0].y >= y) {
                    vectors[x + (z * Constants.CHUNK_SIZE1D)][0].y = vectors[x + (z * Constants.CHUNK_SIZE1D) - 1][2].y;
                    vectors[x + (z * Constants.CHUNK_SIZE1D)][1].y = vectors[x + (z * Constants.CHUNK_SIZE1D) - 1][2].y;
                }
            }

            indice[objectID - 1] += 4;

            //Top
            vectors = vertices[objectID - 1][4];
            vectors[x + (z * Constants.CHUNK_SIZE1D)] = new Position[4];

            //Naive Greedy Meshing
            int sx = x;

            if (x > 0 && vectors[x + (z * Constants.CHUNK_SIZE1D) - 1] != null && vectors[x + (z * Constants.CHUNK_SIZE1D) - 1][0].y == ay) {
                sx = vectors[x + (z * Constants.CHUNK_SIZE1D) - 1][0].x;
                vectors[x + (z * Constants.CHUNK_SIZE1D) - 1] = null;
                indice[objectID - 1] -= 4;
            }

            //1
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].x = sx;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].y = ay;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].z = z;

            //2
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].x = ax;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].y = ay;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].z = z;

            //3
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].x = ax;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].y = ay;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].z = az;

            //4
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].x = sx;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].y = ay;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].z = az;

            indice[objectID - 1] += 4;

            //Bottom
            vectors = vertices[objectID - 1][5];
            vectors[x + (z * Constants.CHUNK_SIZE1D)] = new Position[4];

            //Naive Greedy Meshing
            sx = x;
            if (x > 0 && vectors[x + (z * Constants.CHUNK_SIZE1D) - 1] != null && vectors[x + (z * Constants.CHUNK_SIZE1D) - 1][0].y == y) {
                sx = vectors[x + (z * Constants.CHUNK_SIZE1D) - 1][1].x;
                vectors[x + (z * Constants.CHUNK_SIZE1D) - 1] = null;
                indice[objectID - 1] -= 4;
            }

            //1
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].x = ax;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].y = y;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][0].z = z;

            //2
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].x = sx;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].y = y;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][1].z = z;

            //3
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].x = sx;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].y = y;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][2].z = az;

            //4
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].x = ax;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].y = y;
            vectors[x + (z * Constants.CHUNK_SIZE1D)][3].z = az;

            indice[objectID - 1] += 4;

            count += lenght;
        }

        values.vertices = vertices;
        values.indices = indice;
        return values;
    }

    public static Stack<Position[]> GreedyMeshing (int side, Position[][] primitives, Stack<Position[]> stack) {
        for (int x = 0; x < Constants.CHUNK_SIZE1D; x++) {
            for (int z = 0; z < Constants.CHUNK_SIZE1D; z++) {
                if (primitives[x + (z * Constants.CHUNK_SIZE1D)] != null) {
                    switch (side) {
                        case 0:
                            if (primitives[x + (z * Constants.CHUNK_SIZE1D)][0].delete) {
                                primitives[x + (z * Constants.CHUNK_SIZE1D)] = null;
                                continue;
                            } else if (x < Constants.CHUNK_SIZE1D - 1 && primitives[x + (z * Constants.CHUNK_SIZE1D) + 1] != null) {
                                if (primitives[x + (z * Constants.CHUNK_SIZE1D) + 1][0].delete) {
                                    primitives[x + (z * Constants.CHUNK_SIZE1D) + 1] = null;
                                } else if (primitives[x + (z * Constants.CHUNK_SIZE1D) + 1][0].y == primitives[x + (z * Constants.CHUNK_SIZE1D)][0].y &&
                                    primitives[x + (z * Constants.CHUNK_SIZE1D)][2].y == primitives[x + (z * Constants.CHUNK_SIZE1D) + 1][2].y) {
                                    primitives[x + (z * Constants.CHUNK_SIZE1D)][1].x = primitives[x + (z * Constants.CHUNK_SIZE1D) + 1][1].x;
                                    primitives[x + (z * Constants.CHUNK_SIZE1D)][2].x = primitives[x + (z * Constants.CHUNK_SIZE1D) + 1][2].x;
                                    primitives[x + (z * Constants.CHUNK_SIZE1D) + 1] = primitives[x + (z * Constants.CHUNK_SIZE1D)];
                                    primitives[x + (z * Constants.CHUNK_SIZE1D)] = null;
                                    continue;
                                }
                            }
                            stack.Push (primitives[x + (z * Constants.CHUNK_SIZE1D)]);
                            break;
                        case 1:
                            if (x < Constants.CHUNK_SIZE1D - 1 && primitives[x + (z * Constants.CHUNK_SIZE1D) + 1] != null &&
                                primitives[x + (z * Constants.CHUNK_SIZE1D) + 1][2].y == primitives[x + (z * Constants.CHUNK_SIZE1D)][2].y &&
                                primitives[x + (z * Constants.CHUNK_SIZE1D)][0].y == primitives[x + (z * Constants.CHUNK_SIZE1D) + 1][0].y) {

                                primitives[x + (z * Constants.CHUNK_SIZE1D)][0].x = primitives[x + (z * Constants.CHUNK_SIZE1D) + 1][0].x;
                                primitives[x + (z * Constants.CHUNK_SIZE1D)][3].x = primitives[x + (z * Constants.CHUNK_SIZE1D) + 1][3].x;
                                primitives[x + (z * Constants.CHUNK_SIZE1D) + 1] = primitives[x + (z * Constants.CHUNK_SIZE1D)];
                                primitives[x + (z * Constants.CHUNK_SIZE1D)] = null;
                                continue;
                            }
                            stack.Push (primitives[x + (z * Constants.CHUNK_SIZE1D)]);
                            break;
                        case 2:
                            if (z < Constants.CHUNK_SIZE1D - 1 && primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] != null &&
                                primitives[x + (z * Constants.CHUNK_SIZE1D)][2].y == primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][2].y &&
                                primitives[x + (z * Constants.CHUNK_SIZE1D)][0].y == primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][0].y) {

                                primitives[x + (z * Constants.CHUNK_SIZE1D)][1].z = primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][1].z;
                                primitives[x + (z * Constants.CHUNK_SIZE1D)][2].z = primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][2].z;
                                primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] = primitives[x + (z * Constants.CHUNK_SIZE1D)];
                                primitives[x + (z * Constants.CHUNK_SIZE1D)] = null;
                                continue;
                            }
                            stack.Push (primitives[x + (z * Constants.CHUNK_SIZE1D)]);
                            break;
                        case 3:
                            if (primitives[x + (z * Constants.CHUNK_SIZE1D)][0].delete) {
                                primitives[x + (z * Constants.CHUNK_SIZE1D)] = null;
                                continue;
                            } else if (z < Constants.CHUNK_SIZE1D - 1 && primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] != null) {
                                if (primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][0].delete) {
                                    primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] = null;
                                } else if (primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][0].y == primitives[x + (z * Constants.CHUNK_SIZE1D)][0].y &&
                                    primitives[x + (z * Constants.CHUNK_SIZE1D)][2].y == primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][2].y) {

                                    primitives[x + (z * Constants.CHUNK_SIZE1D)][0].z = primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][0].z;
                                    primitives[x + (z * Constants.CHUNK_SIZE1D)][3].z = primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][3].z;
                                    primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] = primitives[x + (z * Constants.CHUNK_SIZE1D)];
                                    primitives[x + (z * Constants.CHUNK_SIZE1D)] = null;
                                    continue;
                                }
                            }
                            stack.Push (primitives[x + (z * Constants.CHUNK_SIZE1D)]);
                            break;
                        case 4:
                            if (z < Constants.CHUNK_SIZE1D - 1 && primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] != null &&
                                primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][0].x == primitives[x + (z * Constants.CHUNK_SIZE1D)][0].x &&
                                primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][1].x == primitives[x + (z * Constants.CHUNK_SIZE1D)][1].x &&
                                primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][0].y == primitives[x + (z * Constants.CHUNK_SIZE1D)][0].y) {

                                primitives[x + (z * Constants.CHUNK_SIZE1D)][3].z = primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][3].z;
                                primitives[x + (z * Constants.CHUNK_SIZE1D)][2].z = primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][2].z;
                                primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] = primitives[x + (z * Constants.CHUNK_SIZE1D)];
                                primitives[x + (z * Constants.CHUNK_SIZE1D)] = null;
                                continue;
                            }
                            stack.Push (primitives[x + (z * Constants.CHUNK_SIZE1D)]);
                            break;

                        case 5:
                            if (z < Constants.CHUNK_SIZE1D - 1 && primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] != null &&
                                primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][0].x == primitives[x + (z * Constants.CHUNK_SIZE1D)][0].x &&
                                primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][2].x == primitives[x + (z * Constants.CHUNK_SIZE1D)][2].x &&
                                primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][0].y == primitives[x + (z * Constants.CHUNK_SIZE1D)][0].y) {

                                primitives[x + (z * Constants.CHUNK_SIZE1D)][3].z = primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][3].z;
                                primitives[x + (z * Constants.CHUNK_SIZE1D)][2].z = primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][2].z;
                                primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] = primitives[x + (z * Constants.CHUNK_SIZE1D)];
                                primitives[x + (z * Constants.CHUNK_SIZE1D)] = null;
                                continue;
                            }

                            stack.Push (primitives[x + (z * Constants.CHUNK_SIZE1D)]);
                            break;
                    }
                }
            }
        }
        ArrayPool<Position[]>.Shared.Return (primitives);
        return stack;
    }
}