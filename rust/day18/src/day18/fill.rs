use crate::day18::domain;

struct Sizes {
    min_x: i32,
    max_x: i32,
    min_y: i32,
    max_y: i32,
    min_z: i32,
    max_z: i32,
}

fn get_min_and_max_from_outside(input: &[domain::Coordinates]) -> Sizes {
    let first = input.first().unwrap();
    let mut output = Sizes {
        min_x: first.x,
        max_x: first.x,
        min_y: first.y,
        max_y: first.y,
        min_z: first.z,
        max_z: first.z,
    };
    input.iter().for_each(|coord| {
        match coord.x {
            x_min if x_min < output.min_x => output.min_x = x_min,
            x_max if x_max > output.max_x => output.max_x = x_max,
            _ => {}
        }
        match coord.y {
            y_min if y_min < output.min_y => output.min_y = y_min,
            y_max if y_max > output.max_y => output.max_y = y_max,
            _ => {}
        }
        match coord.z {
            z_min if z_min < output.min_z => output.min_z = z_min,
            z_max if z_max > output.max_z => output.max_z = z_max,
            _ => {}
        }
    });
    output.min_x -= 1;
    output.max_x += 1;
    output.min_y -= 1;
    output.max_y += 1;
    output.min_z -= 1;
    output.max_z += 1;
    output
}

fn get_next_voxels<F>(
    current: &domain::Coordinates,
    get_neighbors: F,
    sizes: &Sizes,
    input: &[domain::Coordinates],
    exclude1: &[domain::Coordinates],
    exclude2: &[domain::Coordinates],
) -> Vec<domain::Coordinates>
where
    F: Fn(&domain::Coordinates) -> Vec<domain::Coordinates>,
{
    get_neighbors(current)
        .into_iter()
        .filter(|neighbor| !input.contains(neighbor))
        .filter(|neighbor| !exclude1.contains(neighbor))
        .filter(|neighbor| !exclude2.contains(neighbor))
        .filter(|neighbor| {
            neighbor.x >= sizes.min_x
                && neighbor.x <= sizes.max_x
                && neighbor.y >= sizes.min_y
                && neighbor.y <= sizes.max_y
                && neighbor.z >= sizes.min_z
                && neighbor.z <= sizes.max_z
        })
        .collect::<Vec<domain::Coordinates>>()
}

pub struct NegativeShapeResult {
    pub negative_shape: Vec<domain::Coordinates>,
    pub outer_shell_size: usize,
}

pub fn get_negative_shape_from_outside<F>(
    input: &[domain::Coordinates],
    get_neighbors: F,
) -> NegativeShapeResult
where
    F: Fn(&domain::Coordinates) -> Vec<domain::Coordinates>,
{
    let sizes = get_min_and_max_from_outside(input);
    let mut to_be_calculated = vec![domain::Coordinates {
        x: sizes.min_x,
        y: sizes.min_y,
        z: sizes.min_z,
    }];
    let mut already_calculated = vec![];
    while let Some(current) = to_be_calculated.pop() {
        let next_ones = get_next_voxels(
            &current,
            &get_neighbors,
            &sizes,
            input,
            &to_be_calculated,
            &already_calculated,
        );
        to_be_calculated.extend(next_ones);
        already_calculated.push(current);
    }
    let outer_shell_size = {
        let x_diff = sizes.max_x - sizes.min_x + 1;
        let y_diff = sizes.max_y - sizes.min_y + 1;
        let z_diff = sizes.max_z - sizes.min_z + 1;
        (x_diff * y_diff * 2 + x_diff * z_diff * 2 + y_diff * z_diff * 2) as usize
    };
    NegativeShapeResult {
        negative_shape: already_calculated,
        outer_shell_size,
    }
}

#[cfg(test)]
pub mod tests {
    use super::*;
    use crate::day18::neighbor;
    use std::collections::HashSet;

    #[test]
    fn simple_sizes() {
        let input = vec![domain::Coordinates { x: 1, y: 1, z: 1 }];
        let result = get_min_and_max_from_outside(&input);
        assert_eq!(result.min_x, 0);
        assert_eq!(result.max_x, 2);
        assert_eq!(result.min_y, 0);
        assert_eq!(result.max_y, 2);
        assert_eq!(result.min_z, 0);
        assert_eq!(result.max_z, 2);
    }

    #[test]
    fn simple_negative() {
        let input = vec![domain::Coordinates { x: 1, y: 1, z: 1 }];
        let expected = vec![
            domain::Coordinates { x: 0, y: 0, z: 0 },
            domain::Coordinates { x: 0, y: 1, z: 0 },
            domain::Coordinates { x: 0, y: 2, z: 0 },
            domain::Coordinates { x: 1, y: 0, z: 0 },
            domain::Coordinates { x: 1, y: 1, z: 0 },
            domain::Coordinates { x: 1, y: 2, z: 0 },
            domain::Coordinates { x: 2, y: 0, z: 0 },
            domain::Coordinates { x: 2, y: 1, z: 0 },
            domain::Coordinates { x: 2, y: 2, z: 0 },
            domain::Coordinates { x: 0, y: 0, z: 1 },
            domain::Coordinates { x: 0, y: 1, z: 1 },
            domain::Coordinates { x: 0, y: 2, z: 1 },
            domain::Coordinates { x: 1, y: 0, z: 1 },
            domain::Coordinates { x: 1, y: 2, z: 1 },
            domain::Coordinates { x: 2, y: 0, z: 1 },
            domain::Coordinates { x: 2, y: 1, z: 1 },
            domain::Coordinates { x: 2, y: 2, z: 1 },
            domain::Coordinates { x: 0, y: 0, z: 2 },
            domain::Coordinates { x: 0, y: 1, z: 2 },
            domain::Coordinates { x: 0, y: 2, z: 2 },
            domain::Coordinates { x: 1, y: 0, z: 2 },
            domain::Coordinates { x: 1, y: 1, z: 2 },
            domain::Coordinates { x: 1, y: 2, z: 2 },
            domain::Coordinates { x: 2, y: 0, z: 2 },
            domain::Coordinates { x: 2, y: 1, z: 2 },
            domain::Coordinates { x: 2, y: 2, z: 2 },
        ];
        let result = get_negative_shape_from_outside(&input, neighbor::get_potential_neighbors);
        assert_eq!(result.negative_shape.len(), expected.len());
        assert_eq!(result.outer_shell_size, 54);

        let result_set = result.negative_shape.iter().collect::<HashSet<_>>();
        let expected_set = expected.iter().collect::<HashSet<_>>();
        assert_eq!(result_set.len(), expected_set.len());
        assert_eq!(result_set, expected_set);
    }

    #[test]
    fn sizes_example_01() {
        let input = vec![
            domain::Coordinates { x: 1, y: 1, z: 1 },
            domain::Coordinates { x: 2, y: 2, z: 2 },
            domain::Coordinates { x: 3, y: 3, z: 1 },
        ];
        let result = get_min_and_max_from_outside(&input);
        assert_eq!(result.min_x, 0);
        assert_eq!(result.max_x, 4);
        assert_eq!(result.min_y, 0);
        assert_eq!(result.max_y, 4);
        assert_eq!(result.min_z, 0);
        assert_eq!(result.max_z, 3);
    }

    #[test]
    fn negative_example_01() {
        let input = vec![
            domain::Coordinates { x: 1, y: 1, z: 1 },
            domain::Coordinates { x: 2, y: 1, z: 1 },
            domain::Coordinates { x: 3, y: 1, z: 1 },
            domain::Coordinates { x: 1, y: 2, z: 1 },
            domain::Coordinates { x: 2, y: 2, z: 1 },
            domain::Coordinates { x: 3, y: 2, z: 1 },
            domain::Coordinates { x: 1, y: 3, z: 1 },
            domain::Coordinates { x: 2, y: 3, z: 1 },
            domain::Coordinates { x: 3, y: 3, z: 1 },
            domain::Coordinates { x: 1, y: 1, z: 2 },
            domain::Coordinates { x: 2, y: 1, z: 2 },
            domain::Coordinates { x: 3, y: 1, z: 2 },
            domain::Coordinates { x: 1, y: 2, z: 2 },
            //domain::Coordinates { x: 2, y: 2, z: 2 },
            domain::Coordinates { x: 3, y: 2, z: 2 },
            domain::Coordinates { x: 1, y: 3, z: 2 },
            domain::Coordinates { x: 2, y: 3, z: 2 },
            domain::Coordinates { x: 3, y: 3, z: 2 },
            domain::Coordinates { x: 1, y: 1, z: 3 },
            domain::Coordinates { x: 2, y: 1, z: 3 },
            domain::Coordinates { x: 3, y: 1, z: 3 },
            domain::Coordinates { x: 1, y: 2, z: 3 },
            domain::Coordinates { x: 2, y: 2, z: 3 },
            domain::Coordinates { x: 3, y: 2, z: 3 },
            domain::Coordinates { x: 1, y: 3, z: 3 },
            domain::Coordinates { x: 2, y: 3, z: 3 },
            domain::Coordinates { x: 3, y: 3, z: 3 },
        ];
        let expected = vec![
            domain::Coordinates { x: 0, y: 0, z: 0 },
            domain::Coordinates { x: 1, y: 0, z: 0 },
            domain::Coordinates { x: 2, y: 0, z: 0 },
            domain::Coordinates { x: 3, y: 0, z: 0 },
            domain::Coordinates { x: 4, y: 0, z: 0 },
            domain::Coordinates { x: 0, y: 1, z: 0 },
            domain::Coordinates { x: 1, y: 1, z: 0 },
            domain::Coordinates { x: 2, y: 1, z: 0 },
            domain::Coordinates { x: 3, y: 1, z: 0 },
            domain::Coordinates { x: 4, y: 1, z: 0 },
            domain::Coordinates { x: 0, y: 2, z: 0 },
            domain::Coordinates { x: 1, y: 2, z: 0 },
            domain::Coordinates { x: 2, y: 2, z: 0 },
            domain::Coordinates { x: 3, y: 2, z: 0 },
            domain::Coordinates { x: 4, y: 2, z: 0 },
            domain::Coordinates { x: 0, y: 3, z: 0 },
            domain::Coordinates { x: 1, y: 3, z: 0 },
            domain::Coordinates { x: 2, y: 3, z: 0 },
            domain::Coordinates { x: 3, y: 3, z: 0 },
            domain::Coordinates { x: 4, y: 3, z: 0 },
            domain::Coordinates { x: 0, y: 4, z: 0 },
            domain::Coordinates { x: 1, y: 4, z: 0 },
            domain::Coordinates { x: 2, y: 4, z: 0 },
            domain::Coordinates { x: 3, y: 4, z: 0 },
            domain::Coordinates { x: 4, y: 4, z: 0 },
            domain::Coordinates { x: 0, y: 0, z: 1 },
            domain::Coordinates { x: 1, y: 0, z: 1 },
            domain::Coordinates { x: 2, y: 0, z: 1 },
            domain::Coordinates { x: 3, y: 0, z: 1 },
            domain::Coordinates { x: 4, y: 0, z: 1 },
            domain::Coordinates { x: 0, y: 1, z: 1 },
            domain::Coordinates { x: 4, y: 1, z: 1 },
            domain::Coordinates { x: 0, y: 2, z: 1 },
            domain::Coordinates { x: 4, y: 2, z: 1 },
            domain::Coordinates { x: 0, y: 3, z: 1 },
            domain::Coordinates { x: 4, y: 3, z: 1 },
            domain::Coordinates { x: 0, y: 4, z: 1 },
            domain::Coordinates { x: 1, y: 4, z: 1 },
            domain::Coordinates { x: 2, y: 4, z: 1 },
            domain::Coordinates { x: 3, y: 4, z: 1 },
            domain::Coordinates { x: 4, y: 4, z: 1 },
            domain::Coordinates { x: 0, y: 0, z: 2 },
            domain::Coordinates { x: 1, y: 0, z: 2 },
            domain::Coordinates { x: 2, y: 0, z: 2 },
            domain::Coordinates { x: 3, y: 0, z: 2 },
            domain::Coordinates { x: 4, y: 0, z: 2 },
            domain::Coordinates { x: 0, y: 1, z: 2 },
            domain::Coordinates { x: 4, y: 1, z: 2 },
            domain::Coordinates { x: 0, y: 2, z: 2 },
            domain::Coordinates { x: 4, y: 2, z: 2 },
            domain::Coordinates { x: 0, y: 3, z: 2 },
            domain::Coordinates { x: 4, y: 3, z: 2 },
            domain::Coordinates { x: 0, y: 4, z: 2 },
            domain::Coordinates { x: 1, y: 4, z: 2 },
            domain::Coordinates { x: 2, y: 4, z: 2 },
            domain::Coordinates { x: 3, y: 4, z: 2 },
            domain::Coordinates { x: 4, y: 4, z: 2 },
            domain::Coordinates { x: 0, y: 0, z: 3 },
            domain::Coordinates { x: 1, y: 0, z: 3 },
            domain::Coordinates { x: 2, y: 0, z: 3 },
            domain::Coordinates { x: 3, y: 0, z: 3 },
            domain::Coordinates { x: 4, y: 0, z: 3 },
            domain::Coordinates { x: 0, y: 1, z: 3 },
            domain::Coordinates { x: 4, y: 1, z: 3 },
            domain::Coordinates { x: 0, y: 2, z: 3 },
            domain::Coordinates { x: 4, y: 2, z: 3 },
            domain::Coordinates { x: 0, y: 3, z: 3 },
            domain::Coordinates { x: 4, y: 3, z: 3 },
            domain::Coordinates { x: 0, y: 4, z: 3 },
            domain::Coordinates { x: 1, y: 4, z: 3 },
            domain::Coordinates { x: 2, y: 4, z: 3 },
            domain::Coordinates { x: 3, y: 4, z: 3 },
            domain::Coordinates { x: 4, y: 4, z: 3 },
            domain::Coordinates { x: 0, y: 0, z: 4 },
            domain::Coordinates { x: 1, y: 0, z: 4 },
            domain::Coordinates { x: 2, y: 0, z: 4 },
            domain::Coordinates { x: 3, y: 0, z: 4 },
            domain::Coordinates { x: 4, y: 0, z: 4 },
            domain::Coordinates { x: 0, y: 1, z: 4 },
            domain::Coordinates { x: 1, y: 1, z: 4 },
            domain::Coordinates { x: 2, y: 1, z: 4 },
            domain::Coordinates { x: 3, y: 1, z: 4 },
            domain::Coordinates { x: 4, y: 1, z: 4 },
            domain::Coordinates { x: 0, y: 2, z: 4 },
            domain::Coordinates { x: 1, y: 2, z: 4 },
            domain::Coordinates { x: 2, y: 2, z: 4 },
            domain::Coordinates { x: 3, y: 2, z: 4 },
            domain::Coordinates { x: 4, y: 2, z: 4 },
            domain::Coordinates { x: 0, y: 3, z: 4 },
            domain::Coordinates { x: 1, y: 3, z: 4 },
            domain::Coordinates { x: 2, y: 3, z: 4 },
            domain::Coordinates { x: 3, y: 3, z: 4 },
            domain::Coordinates { x: 4, y: 3, z: 4 },
            domain::Coordinates { x: 0, y: 4, z: 4 },
            domain::Coordinates { x: 1, y: 4, z: 4 },
            domain::Coordinates { x: 2, y: 4, z: 4 },
            domain::Coordinates { x: 3, y: 4, z: 4 },
            domain::Coordinates { x: 4, y: 4, z: 4 },
        ];
        let result = get_negative_shape_from_outside(&input, neighbor::get_potential_neighbors);
        assert_eq!(result.negative_shape.len(), expected.len());
        assert_eq!(result.outer_shell_size, 150);

        let result_set = result.negative_shape.iter().collect::<HashSet<_>>();
        let expected_set = expected.iter().collect::<HashSet<_>>();
        assert_eq!(result_set.len(), expected_set.len());
        assert_eq!(result_set, expected_set);
    }
}
