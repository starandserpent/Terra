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
                    vertices[objectID - 1][s] = new Position[Constants.CHUNK_SIZE1D * Constants.CHUNK_SIZE1D][];
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
            Position[] positions = vectors[x + (z * Constants.CHUNK_SIZE1D)];

            //1
            positions[0].x = x;
            positions[0].y = y;
            positions[0].z = z;

            //2
            positions[1].x = ax;
            positions[1].y = y;
            positions[1].z = z;

            //3
            positions[2].x = ax;
            positions[2].y = ay;
            positions[2].z = z;

            //4
            positions[3].x = x;
            positions[3].y = ay;
            positions[3].z = z;

            if (z > 0) {
                Position[] sidePosition = vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D];
                if (sidePosition != null) {
                    if (sidePosition[2].y >= ay) {
                        for (int s = 0; s < 4; s++) {
                            positions[s].delete = true;
                        }
                    } else if (sidePosition[2].y < ay && sidePosition[0].y >= y) {
                        positions[0].y = sidePosition[2].y;
                        positions[1].y = sidePosition[2].y;
                    }
                }
            }

            indice[objectID - 1] += 4;

            //Back
            vectors = vertices[objectID - 1][1];
            vectors[x + (z * Constants.CHUNK_SIZE1D)] = new Position[4];
            positions = vectors[x + (z * Constants.CHUNK_SIZE1D)];

            //1
            positions[0].x = ax;
            positions[0].y = y;
            positions[0].z = az;

            //2
            positions[1].x = x;
            positions[1].y = y;
            positions[1].z = az;

            //3
            positions[2].x = x;
            positions[2].y = ay;
            positions[2].z = az;

            //4
            positions[3].x = ax;
            positions[3].y = ay;
            positions[3].z = az;

            if (z > 0) {
                Position[] sidePosition = vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D];
                if (sidePosition != null) {
                    if (sidePosition[2].y > ay && sidePosition[0].y <= y) {
                        sidePosition[0].y = ay;
                        sidePosition[1].y = ay;
                    } else if (sidePosition[2].y <= ay) {
                        vectors[x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D] = null;
                        indice[objectID - 1] -= 4;
                    }
                }
            }

            indice[objectID - 1] += 4;

            //Right
            vectors = vertices[objectID - 1][2];
            vectors[x + (z * Constants.CHUNK_SIZE1D)] = new Position[4];
            positions = vectors[x + (z * Constants.CHUNK_SIZE1D)];

            //1
            positions[0].x = ax;
            positions[0].y = y;
            positions[0].z = z;

            //2
            positions[1].x = ax;
            positions[1].y = y;
            positions[1].z = az;

            //3
            positions[2].x = ax;
            positions[2].y = ay;
            positions[2].z = az;

            //4
            positions[3].x = ax;
            positions[3].y = ay;
            positions[3].z = z;

            if (x > 0) {
                Position[] sidePosition = vectors[x + (z * Constants.CHUNK_SIZE1D) - 1];
                if (sidePosition != null) {
                    if (sidePosition[2].y > ay && sidePosition[0].y <= y) {
                        sidePosition[0].y = ay;
                        sidePosition[1].y = ay;
                    } else if (sidePosition[2].y <= ay) {
                        vectors[x + (z * Constants.CHUNK_SIZE1D) - 1] = null;
                        indice[objectID - 1] -= 4;
                    }
                }
            }

            indice[objectID - 1] += 4;

            //Left
            vectors = vertices[objectID - 1][3];
            vectors[x + (z * Constants.CHUNK_SIZE1D)] = new Position[4];
            positions = vectors[x + (z * Constants.CHUNK_SIZE1D)];

            //1
            positions[0].x = x;
            positions[0].y = y;
            positions[0].z = az;

            //2
            positions[1].x = x;
            positions[1].y = y;
            positions[1].z = z;

            //3
            positions[2].x = x;
            positions[2].y = ay;
            positions[2].z = z;

            //4
            positions[3].x = x;
            positions[3].y = ay;
            positions[3].z = az;

            if (x > 0) {
                Position[] sidePosition = vectors[x + (z * Constants.CHUNK_SIZE1D) - 1];
                if (sidePosition != null) {
                    if (sidePosition[2].y >= ay) {
                        for (int s = 0; s < 4; s++) {
                            vectors[x + (z * Constants.CHUNK_SIZE1D)][s].delete = true;
                        }
                    } else if (sidePosition[2].y < ay && sidePosition[0].y >= y) {
                        positions[0].y = sidePosition[2].y;
                        positions[1].y = sidePosition[2].y;
                    }
                }
            }

            indice[objectID - 1] += 4;

            //Top
            vectors = vertices[objectID - 1][4];
            vectors[x + (z * Constants.CHUNK_SIZE1D)] = new Position[4];
            positions = vectors[x + (z * Constants.CHUNK_SIZE1D)];
            //Naive Greedy Meshing
            int sx = x;

            if (x > 0) {
                Position[] sidePosition = vectors[x + (z * Constants.CHUNK_SIZE1D) - 1];
                if (sidePosition != null && sidePosition[0].y == ay) {
                    sx = sidePosition[0].x;
                    vectors[x + (z * Constants.CHUNK_SIZE1D) - 1] = null;
                    indice[objectID - 1] -= 4;
                }
            }

            //1
            positions[0].x = sx;
            positions[0].y = ay;
            positions[0].z = z;

            //2
            positions[1].x = ax;
            positions[1].y = ay;
            positions[1].z = z;

            //3
            positions[2].x = ax;
            positions[2].y = ay;
            positions[2].z = az;

            //4
            positions[3].x = sx;
            positions[3].y = ay;
            positions[3].z = az;

            indice[objectID - 1] += 4;

            //Bottom
            vectors = vertices[objectID - 1][5];
            vectors[x + (z * Constants.CHUNK_SIZE1D)] = new Position[4];
            positions = vectors[x + (z * Constants.CHUNK_SIZE1D)];

            //Naive Greedy Meshing
            sx = x;
            if (x > 0) {
                Position[] sidePosition = vectors[x + (z * Constants.CHUNK_SIZE1D) - 1];
                if (sidePosition != null && sidePosition[0].y == y) {
                    sx = sidePosition[1].x;
                    vectors[x + (z * Constants.CHUNK_SIZE1D) - 1] = null;
                    indice[objectID - 1] -= 4;
                }
            }

            //1
            positions[0].x = ax;
            positions[0].y = y;
            positions[0].z = z;

            //2
            positions[1].x = sx;
            positions[1].y = y;
            positions[1].z = z;

            //3
            positions[2].x = sx;
            positions[2].y = y;
            positions[2].z = az;

            //4
            positions[3].x = ax;
            positions[3].y = y;
            positions[3].z = az;

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
                Position[] positions = primitives[x + (z * Constants.CHUNK_SIZE1D)];
                if (positions != null) {
                    switch (side) {
                        case 0:
                            if (x < Constants.CHUNK_SIZE1D - 1) {
                                Position[] sidePosition = primitives[x + (z * Constants.CHUNK_SIZE1D) + 1];
                                if (sidePosition != null) {
                                    if (sidePosition[0].delete) {
                                        primitives[x + (z * Constants.CHUNK_SIZE1D) + 1] = null;
                                    } else if (sidePosition[0].y == positions[0].y &&
                                        positions[2].y == sidePosition[2].y) {

                                        positions[1].x = sidePosition[1].x;
                                        positions[2].x = sidePosition[2].x;
                                        primitives[x + (z * Constants.CHUNK_SIZE1D) + 1] = positions;
                                        primitives[x + (z * Constants.CHUNK_SIZE1D)] = null;
                                        continue;
                                    }
                                }
                            }
                            stack.Push (positions);
                            break;
                        case 1:
                            if (x < Constants.CHUNK_SIZE1D - 1) {
                                Position[] sidePosition = primitives[x + (z * Constants.CHUNK_SIZE1D) + 1];
                                if (sidePosition != null &&
                                    sidePosition[2].y == primitives[x + (z * Constants.CHUNK_SIZE1D)][2].y &&
                                    positions[0].y == sidePosition[0].y) {

                                    positions[0].x = sidePosition[0].x;
                                    positions[3].x = sidePosition[3].x;
                                    primitives[x + (z * Constants.CHUNK_SIZE1D) + 1] = positions;
                                    primitives[x + (z * Constants.CHUNK_SIZE1D)] = null;
                                    continue;
                                }
                            }
                            stack.Push (positions);
                            break;
                        case 2:
                            if (z < Constants.CHUNK_SIZE1D - 1) {
                                Position[] sidePosition = primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D];
                                if (sidePosition != null &&
                                    positions[2].y == sidePosition[2].y &&
                                    positions[0].y == sidePosition[0].y) {

                                    positions[1].z = sidePosition[1].z;
                                    positions[2].z = sidePosition[2].z;
                                    primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] = positions;
                                    primitives[x + (z * Constants.CHUNK_SIZE1D)] = null;
                                    continue;
                                }
                            }
                            stack.Push (positions);
                            break;
                        case 3:
                            if (z < Constants.CHUNK_SIZE1D - 1) {
                                Position[] sidePosition = primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D];
                                if (sidePosition != null) {
                                    if (sidePosition[0].delete) {
                                        primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] = null;
                                    } else if (sidePosition[0].y == positions[0].y && sidePosition[2].y == positions[2].y) {
                                        positions[0].z = sidePosition[0].z;
                                        positions[3].z = sidePosition[3].z;
                                        primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] = positions;
                                        primitives[x + (z * Constants.CHUNK_SIZE1D)] = null;
                                        continue;
                                    }
                                }
                            }
                            stack.Push (positions);
                            break;
                        case 4:
                            if (z < Constants.CHUNK_SIZE1D - 1) {
                                Position[] sidePosition = primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D];
                                if (sidePosition != null &&
                                    sidePosition[0].x == positions[0].x && sidePosition[1].x == positions[1].x &&
                                    sidePosition[0].y == positions[0].y) {

                                    positions[3].z = sidePosition[3].z;
                                    positions[2].z = sidePosition[2].z;
                                    primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] = positions;
                                    primitives[x + (z * Constants.CHUNK_SIZE1D)] = null;
                                    continue;
                                }
                            }
                            stack.Push (positions);
                            break;

                        case 5:
                            if (z < Constants.CHUNK_SIZE1D - 1) {
                                Position[] sidePosition = primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D];
                                if (sidePosition != null &&
                                    sidePosition[0].x == positions[0].x &&
                                    sidePosition[2].x == positions[2].x &&
                                    sidePosition[0].y == positions[0].y) {

                                    positions[3].z = sidePosition[3].z;
                                    positions[2].z = sidePosition[2].z;
                                    primitives[x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] = positions;
                                    primitives[x + (z * Constants.CHUNK_SIZE1D)] = null;
                                    continue;
                                }
                            }

                            stack.Push (positions);
                            break;
                    }
                }
            }
        }
        return stack;
    }
}