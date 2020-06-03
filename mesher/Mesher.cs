using System.Collections.Generic;
using System.Linq;
public class Mesher {
    public static Position[][][][] NaiveGreedyMeshing (Chunk chunk) {
        Position[][][][] vertices = new Position[6][][][];
        for (int s = 0; s < 6; s++) {
            vertices[s] = new Position[Constants.CHUNK_SIZE2D][][];
        }

        int count = 0;
        int prev = 0;
        int objectID = 0;
        int lenght;

        for (int i = 1; count < Constants.CHUNK_SIZE3D; i++) {

            if (prev > 0) {
                i--;
                lenght = prev;
                prev = 0;
            } else {
                Run run = chunk.Voxels[chunk.Voxels.Count () - i];
                objectID = run.value;
                lenght = run.lenght;
            }

            if (objectID == 0) {
                count += lenght;
                continue;
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

            for (int side = 0; side < 6; side++) {
                if (vertices[side][x + (z * Constants.CHUNK_SIZE1D)] == null) {
                    vertices[side][x + (z * Constants.CHUNK_SIZE1D)] = new Position[chunk.Materials - 1][];
                }
            }

            if (ay != y) {
                //Front
                Position[] positions;
                int index = 0;
                while (vertices[0][x + (z * Constants.CHUNK_SIZE1D)][index] != null) {
                    index++;
                }

                vertices[0][x + (z * Constants.CHUNK_SIZE1D)][index] = new Position[4];
                positions = vertices[0][x + (z * Constants.CHUNK_SIZE1D)][index];

                //1
                positions[0].x = x;
                positions[0].y = y;
                positions[0].z = z;
                positions[0].id = objectID;

                //2
                positions[1].x = ax;
                positions[1].y = y;
                positions[1].z = z;
                positions[1].id = objectID;

                //3
                positions[2].x = ax;
                positions[2].y = ay;
                positions[2].z = z;
                positions[2].id = objectID;

                //4
                positions[3].x = x;
                positions[3].y = ay;
                positions[3].z = z;
                positions[3].id = objectID;

                for (index = 0; index < chunk.Materials - 1; index++) {
                    if (z > 0 && vertices[0][x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D] != null) {
                        Position[] sidePosition = vertices[0][x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D][index];
                        if (sidePosition != null) {
                            if (sidePosition[2].y >= ay) {
                                for (int s = 0; s < 4; s++) {
                                    positions[s].id = 0;
                                }
                            } else if (sidePosition[2].y < ay && sidePosition[0].y >= y) {
                                positions[0].y = sidePosition[2].y;
                                positions[1].y = sidePosition[2].y;
                            }
                        }
                    }
                }

                //Back
                index = 0;
                while (vertices[1][x + (z * Constants.CHUNK_SIZE1D)][index] != null) {
                    index++;
                }

                vertices[1][x + (z * Constants.CHUNK_SIZE1D)][index] = new Position[4];
                positions = vertices[1][x + (z * Constants.CHUNK_SIZE1D)][index];

                //1
                positions[0].x = ax;
                positions[0].y = y;
                positions[0].z = az;
                positions[0].id = objectID;

                //2
                positions[1].x = x;
                positions[1].y = y;
                positions[1].z = az;
                positions[1].id = objectID;

                //3
                positions[2].x = x;
                positions[2].y = ay;
                positions[2].z = az;
                positions[2].id = objectID;

                //4
                positions[3].x = ax;
                positions[3].y = ay;
                positions[3].z = az;
                positions[3].id = objectID;

                for (index = 0; index < chunk.Materials - 1; index++) {
                    if (z > 0 && vertices[1][x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D] != null) {
                        Position[] sidePosition = vertices[1][x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D][index];
                        if (sidePosition != null) {
                            if (sidePosition[2].y > ay && sidePosition[0].y <= y) {
                                sidePosition[0].y = ay;
                                sidePosition[1].y = ay;
                            } else if (sidePosition[2].y <= ay) {
                                vertices[1][x + (z * Constants.CHUNK_SIZE1D) - Constants.CHUNK_SIZE1D][index] = null;
                            }
                        }
                    }
                }

                //Right
                index = 0;
                while (vertices[2][x + (z * Constants.CHUNK_SIZE1D)][index] != null) {
                    index++;
                }

                vertices[2][x + (z * Constants.CHUNK_SIZE1D)][index] = new Position[4];
                positions = vertices[2][x + (z * Constants.CHUNK_SIZE1D)][index];

                //1
                positions[0].x = ax;
                positions[0].y = y;
                positions[0].z = z;
                positions[0].id = objectID;

                //2
                positions[1].x = ax;
                positions[1].y = y;
                positions[1].z = az;
                positions[1].id = objectID;

                //3
                positions[2].x = ax;
                positions[2].y = ay;
                positions[2].z = az;
                positions[2].id = objectID;

                //4
                positions[3].x = ax;
                positions[3].y = ay;
                positions[3].z = z;
                positions[3].id = objectID;

                for (index = 0; index < chunk.Materials - 1; index++) {
                    if (x > 0 && vertices[2][x + (z * Constants.CHUNK_SIZE1D) - 1] != null) {
                        Position[] sidePosition = vertices[2][x + (z * Constants.CHUNK_SIZE1D) - 1][index];
                        if (sidePosition != null) {
                            if (sidePosition[2].y > ay && sidePosition[0].y <= y) {
                                sidePosition[0].y = ay;
                                sidePosition[1].y = ay;
                            } else if (sidePosition[2].y <= ay) {
                                vertices[2][x + (z * Constants.CHUNK_SIZE1D) - 1][index] = null;
                            }
                        }
                    }
                }

                //Left
                index = 0;
                while (vertices[3][x + (z * Constants.CHUNK_SIZE1D)][index] != null) {
                    index++;
                }

                vertices[3][x + (z * Constants.CHUNK_SIZE1D)][index] = new Position[4];
                positions = vertices[3][x + (z * Constants.CHUNK_SIZE1D)][index];

                //1
                positions[0].x = x;
                positions[0].y = y;
                positions[0].z = az;
                positions[0].id = objectID;

                //2
                positions[1].x = x;
                positions[1].y = y;
                positions[1].z = z;
                positions[1].id = objectID;

                //3
                positions[2].x = x;
                positions[2].y = ay;
                positions[2].z = z;
                positions[2].id = objectID;

                //4
                positions[3].x = x;
                positions[3].y = ay;
                positions[3].z = az;
                positions[3].id = objectID;

                for (index = 0; index < chunk.Materials - 1; index++) {
                    if (x > 0 && vertices[3][x + (z * Constants.CHUNK_SIZE1D) - 1] != null) {
                        Position[] sidePosition = vertices[3][x + (z * Constants.CHUNK_SIZE1D) - 1][index];
                        if (sidePosition != null) {
                            if (sidePosition[2].y >= ay) {
                                for (int s = 0; s < 4; s++) {
                                    positions[s].id = 0;
                                }
                            } else if (sidePosition[2].y < ay && sidePosition[0].y >= y) {
                                positions[0].y = sidePosition[2].y;
                                positions[1].y = sidePosition[2].y;
                            }
                        }
                    }
                }

                //Top
                index = 0;
                while (vertices[4][x + (z * Constants.CHUNK_SIZE1D)][index] != null) {
                    index++;
                }

                vertices[4][x + (z * Constants.CHUNK_SIZE1D)][index] = new Position[4];
                positions = vertices[4][x + (z * Constants.CHUNK_SIZE1D)][index];

                if (y > 0 && vertices[4][x + (z * Constants.CHUNK_SIZE1D)] != null && index > 0) {
                    Position[] sidePosition = vertices[4][x + (z * Constants.CHUNK_SIZE1D)][index - 1];
                    if (sidePosition != null) {
                        vertices[4][x + (z * Constants.CHUNK_SIZE1D)][index - 1] = null;
                    }
                }

                //Naive Greedy Meshing
                int sx = x;
                if (x > 0 && vertices[4][x + (z * Constants.CHUNK_SIZE1D) - 1] != null) {
                    Position[] sidePosition = vertices[4][x + (z * Constants.CHUNK_SIZE1D) - 1][index];
                    if (sidePosition != null && sidePosition[0].y == ay) {
                        sx = sidePosition[0].x;
                        vertices[4][x + (z * Constants.CHUNK_SIZE1D) - 1] = null;
                    }
                }

                //1
                positions[0].x = sx;
                positions[0].y = ay;
                positions[0].z = z;
                positions[0].id = objectID;

                //2
                positions[1].x = ax;
                positions[1].y = ay;
                positions[1].z = z;
                positions[1].id = objectID;

                //3
                positions[2].x = ax;
                positions[2].y = ay;
                positions[2].z = az;
                positions[2].id = objectID;

                //4
                positions[3].x = sx;
                positions[3].y = ay;
                positions[3].z = az;
                positions[3].id = objectID;

                //Bottom
                index = 0;
                while (vertices[5][x + (z * Constants.CHUNK_SIZE1D)][index] != null) {
                    index++;
                }

                vertices[5][x + (z * Constants.CHUNK_SIZE1D)][index] = new Position[4];
                positions = vertices[5][x + (z * Constants.CHUNK_SIZE1D)][index];

                if (y > 0 && vertices[5][x + (z * Constants.CHUNK_SIZE1D)] != null && index > 0) {
                    Position[] sidePosition = vertices[5][x + (z * Constants.CHUNK_SIZE1D)][index - 1];
                    if (sidePosition != null) {
                        vertices[5][x + (z * Constants.CHUNK_SIZE1D)][index] = vertices[5][x + (z * Constants.CHUNK_SIZE1D)][index - 1];
                        count += lenght;
                        continue;
                    }
                }

                //Naive Greedy Meshing
                sx = x;
                for (index = 0; index < chunk.Materials - 1; index++) {
                    if (x > 0 && vertices[5][x + (z * Constants.CHUNK_SIZE1D) - 1] != null) {
                        Position[] sidePosition = vertices[5][x + (z * Constants.CHUNK_SIZE1D) - 1][index];
                        if (sidePosition != null && sidePosition[0].y == y) {
                            sx = sidePosition[1].x;
                            vertices[5][x + (z * Constants.CHUNK_SIZE1D) - 1] = null;
                        }
                    }
                }

                //1
                positions[0].x = ax;
                positions[0].y = y;
                positions[0].z = z;
                positions[0].id = objectID;

                //2
                positions[1].x = sx;
                positions[1].y = y;
                positions[1].z = z;
                positions[1].id = objectID;

                //3
                positions[2].x = sx;
                positions[2].y = y;
                positions[2].z = az;
                positions[2].id = objectID;

                //4
                positions[3].x = ax;
                positions[3].y = y;
                positions[3].z = az;
                positions[3].id = objectID;
            }

            count += lenght;
        }

        return vertices;
    }

    public static Stack<Position[]>[] GreedyMeshing (Position[][][][] primitives, int side, Stack<Position[]>[] stack) {
        for (int z = 0; z < Constants.CHUNK_SIZE1D; z++) {
            for (int x = 0; x < Constants.CHUNK_SIZE1D; x++) {
                for (int y = 0; y < stack.Count (); y++) {
                    if (primitives[side][x + (z * Constants.CHUNK_SIZE1D)] != null) {
                        Position[] positions = primitives[side][x + (z * Constants.CHUNK_SIZE1D)][y];
                        if (positions != null) {
                            switch (side) {
                                case 0:
                                    if (positions[0].id == 0) {
                                        primitives[side][x + (z * Constants.CHUNK_SIZE1D)][y] = null;
                                        continue;
                                    } else if (x < Constants.CHUNK_SIZE1D - 1 && primitives[side][x + (z * Constants.CHUNK_SIZE1D) + 1] != null) {
                                        Position[] sidePosition = primitives[side][x + (z * Constants.CHUNK_SIZE1D) + 1][y];
                                        if (sidePosition != null) {
                                            if (sidePosition[0].id == 0) {
                                                primitives[side][x + (z * Constants.CHUNK_SIZE1D) + 1][y] = null;
                                            } else if (sidePosition[0].y == positions[0].y &&
                                                positions[2].y == sidePosition[2].y) {

                                                positions[1].x = sidePosition[1].x;
                                                positions[2].x = sidePosition[2].x;
                                                primitives[side][x + (z * Constants.CHUNK_SIZE1D) + 1][y] = positions;
                                                primitives[side][x + (z * Constants.CHUNK_SIZE1D)][y] = null;
                                                continue;
                                            }
                                        }
                                    }
                                    stack[positions[0].id - 1].Push (positions);
                                    break;
                                case 1:
                                    if (x < Constants.CHUNK_SIZE1D - 1) {
                                        if (primitives[side][x + (z * Constants.CHUNK_SIZE1D) + 1] != null) {
                                            Position[] sidePosition = primitives[side][x + (z * Constants.CHUNK_SIZE1D) + 1][y];
                                            if (sidePosition != null &&
                                                sidePosition[2].y == positions[2].y &&
                                                positions[0].y == sidePosition[0].y) {

                                                positions[0].x = sidePosition[0].x;
                                                positions[3].x = sidePosition[3].x;
                                                primitives[side][x + (z * Constants.CHUNK_SIZE1D) + 1][y] = positions;
                                                primitives[side][x + (z * Constants.CHUNK_SIZE1D)][y] = null;
                                                continue;
                                            }
                                        }
                                    }
                                    stack[positions[0].id - 1].Push (positions);
                                    break;
                                case 2:
                                    if (z < Constants.CHUNK_SIZE1D - 1 && primitives[side][x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] != null) {
                                        Position[] sidePosition = primitives[side][x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][y];
                                        if (sidePosition != null &&
                                            positions[2].y == sidePosition[2].y &&
                                            positions[0].y == sidePosition[0].y) {

                                            positions[1].z = sidePosition[1].z;
                                            positions[2].z = sidePosition[2].z;
                                            primitives[side][x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][y] = positions;
                                            primitives[side][x + (z * Constants.CHUNK_SIZE1D)][y] = null;
                                            continue;
                                        }
                                    }
                                    stack[positions[0].id - 1].Push (positions);
                                    break;
                                case 3:
                                    if (positions[0].id == 0) {
                                        primitives[side][x + (z * Constants.CHUNK_SIZE1D)][y] = null;
                                        continue;
                                    } else if (z < Constants.CHUNK_SIZE1D - 1 && primitives[side][x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] != null) {
                                        Position[] sidePosition = primitives[side][x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][y];
                                        if (sidePosition != null) {
                                            if (sidePosition[0].id == 0) {
                                                primitives[side][x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][y] = null;
                                            } else if (sidePosition[0].y == positions[0].y
                                             && sidePosition[2].y == positions[2].y) {

                                                positions[0].z = sidePosition[0].z;
                                                positions[3].z = sidePosition[3].z;
                                                primitives[side][x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][y] = positions;
                                                primitives[side][x + (z * Constants.CHUNK_SIZE1D)][y] = null;
                                                continue;
                                            }
                                        }
                                    }
                                    stack[positions[0].id - 1].Push (positions);
                                    break;
                                case 4:
                                    if (z < Constants.CHUNK_SIZE1D - 1 && primitives[side][x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] != null) {
                                        Position[] sidePosition = primitives[side][x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][y];
                                        if (sidePosition != null &&
                                            sidePosition[0].x == positions[0].x && sidePosition[1].x == positions[1].x &&
                                            sidePosition[0].y == positions[0].y) {

                                            positions[3].z = sidePosition[3].z;
                                            positions[2].z = sidePosition[2].z;
                                            primitives[side][x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][y] = positions;
                                            primitives[side][x + (z * Constants.CHUNK_SIZE1D)][y] = null;
                                            continue;
                                        }
                                    }
                                    stack[positions[0].id - 1].Push (positions);
                                    break;

                                case 5:
                                    if (z < Constants.CHUNK_SIZE1D - 1 && primitives[side][x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D] != null) {
                                        Position[] sidePosition = primitives[side][x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][y];
                                        if (sidePosition != null &&
                                            sidePosition[0].x == positions[0].x &&
                                            sidePosition[2].x == positions[2].x &&
                                            sidePosition[0].y == positions[0].y) {

                                            positions[3].z = sidePosition[3].z;
                                            positions[2].z = sidePosition[2].z;
                                            primitives[side][x + (z * Constants.CHUNK_SIZE1D) + Constants.CHUNK_SIZE1D][y] = positions;
                                            primitives[side][x + (z * Constants.CHUNK_SIZE1D)][y] = null;
                                            continue;
                                        }
                                    }
                                    stack[positions[0].id - 1].Push (positions);
                                    break;
                            }
                        }
                    }
                }
            }
        }
        return stack;
    }
}