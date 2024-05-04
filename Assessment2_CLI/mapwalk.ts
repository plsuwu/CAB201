type Point = {
    x: number;
    y: number;
};

const map = ["....S.", "......", "......", "......", "E.....", "......"];

const dir = [
    [-1, 0],
    [1, 0],
    [0, -1],
    [0, 1],
];

function walk(map: string[], wall: string, curr: Point, end: Point, seen: boolean[][], path: Point[]): boolean {
    // base cases
	if (curr.x < 0 || curr.x >= map[0].length ||
        curr.y < 0 || curr.y >= map.length) {
		return false;
	}
	if (map[curr.y][curr.x] === wall) {
		return false;
	}
	if (curr.x === end.x && curr.y === end.y) {
		path.push(end);
		return true;
	}
	if (seen[curr.y][curr.x]) {
		return false;
	}

    // pre
	seen[curr.y][curr.x] = true;
	path.push(curr);

    // recurse
	for (let i = 0; i < dir.length; ++i) {
		const [x, y] = dir[i];
		if (walk(map, wall, { x: curr.x + x, y: curr.y + y,}, end, seen, path)) {
			return true;
		}
	}

    // post
	path.pop();
	return false;
}

function solve(map: string[]): boolean[][] {
    const seen: boolean[][] = [];
    // const path: Point[] = [];
    // console.log(path);

    for (let i = 0; i < map.length; i++) {
        seen.push(new Array(map[0].length).fill(false));
    }

    return [[true]];
}

