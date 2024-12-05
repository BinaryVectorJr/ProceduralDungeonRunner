This is a recursive procedural dungeon generator, that generates an infinite runner course, with 3 dungeon chunks at a time. Each chunk is its own dungeon, and once the player picks up a key, they activate the next chunk. After 3 chunks, the original first chunk gets deleted and the chunk stack shifts left in chunk array index. 

Currently there is a problem with custom collisions with the player character, and it falls through the ground. Possibly can be solved by multithreaded chunk generation (future direction).
