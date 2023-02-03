use stopwatch::Stopwatch;

mod day14;

fn main() {
    let sw = Stopwatch::start_new();
    for _ in 1..=50 {
        let input = include_str!("input.txt");

        let number_of_sand_units_that_settle = day14::get_number_of_sand_units_that_settle(input);
        println!("Number of settled sand units: {number_of_sand_units_that_settle}");

        let number_of_sand_units_that_settle_with_floor =
            day14::get_number_of_sand_units_that_settle_with_floor(input);
        println!("Number of settled sand units, including floor: {number_of_sand_units_that_settle_with_floor}");
    }

    println!("Elapsed time: {} ms", sw.elapsed_ms());
}
