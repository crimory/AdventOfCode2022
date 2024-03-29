mod day17;

fn main() {
    let input = include_str!("input.txt");

    let tower_height = day17::how_tall_tower(2022, input);
    println!("Height of the tower: {tower_height} after 2022 rounds");

    let tower_height = day17::how_tall_tower(1_000_000_000_000, input);
    println!("Height of the tower: {tower_height} after 1 000 000 000 000 rounds");
}
