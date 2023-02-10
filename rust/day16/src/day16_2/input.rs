#[derive(Debug, PartialEq)]
struct ValveInternal {
    name: String,
    flow_rate: u32,
    tunnels: Vec<String>,
}

pub struct Valve {
    pub index: u32,
    pub flow_rate: u32,
    pub tunnel_indices: Vec<u32>,
}

pub struct Map {
    pub valves: Vec<Valve>,
    pub valve_labels: Vec<String>,
}
impl Map {
    pub fn get_valve_by_index(&self, index: &u32) -> &Valve {
        self.valves.iter().find(|v| v.index == *index).unwrap()
    }
}

fn read_input_line(input: &str) -> ValveInternal {
    let parts = input
        .split("has flow rate=")
        .flat_map(|s| s.trim().split("; tunnels lead to valves"))
        .flat_map(|s| s.trim().split("; tunnel leads to valve"))
        .flat_map(|s| s.trim().split("Valve"))
        .map(|s| s.trim())
        .filter(|s| !s.is_empty())
        .collect::<Vec<&str>>();
    ValveInternal {
        name: parts[0].to_owned(),
        flow_rate: parts[1].parse::<u32>().unwrap(),
        tunnels: parts[2].split(',').map(|s| s.trim().to_owned()).collect(),
    }
}

fn get_index(labels: &[String], value: &str) -> u32 {
    labels.iter().position(|s| s == value).unwrap() as u32
}

pub fn read_input(input: &str) -> Map {
    let internal_valves = input
        .lines()
        .map(read_input_line)
        .collect::<Vec<ValveInternal>>();
    let mut valve_labels = internal_valves
        .iter()
        .map(|v| v.name.to_owned())
        .collect::<Vec<String>>();
    valve_labels.sort_by_key(|s| s.to_owned());
    let get_labels_index = |valve_name| get_index(&valve_labels, valve_name);
    let valves = internal_valves
        .iter()
        .map(|v| Valve {
            index: get_labels_index(&v.name),
            flow_rate: v.flow_rate,
            tunnel_indices: v.tunnels.iter().map(|t| get_labels_index(t)).collect(),
        })
        .collect();
    Map {
        valve_labels,
        valves,
    }
}

#[cfg(test)]
pub mod tests {
    use super::*;

    const INPUT_LINE: &str = "\
Valve AA has flow rate=0; tunnels lead to valves DD, II, BB";
    pub const INPUT: &str = "\
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II";

    fn get_valve_labels() -> Vec<String> {
        vec![
            "AA".to_owned(),
            "BB".to_owned(),
            "CC".to_owned(),
            "DD".to_owned(),
            "EE".to_owned(),
            "FF".to_owned(),
            "GG".to_owned(),
            "HH".to_owned(),
            "II".to_owned(),
            "JJ".to_owned(),
        ]
    }

    #[test]
    fn read_line_correctly() {
        let expected = ValveInternal {
            name: "AA".to_owned(),
            flow_rate: 0,
            tunnels: vec!["DD".to_owned(), "II".to_owned(), "BB".to_owned()],
        };
        let result = read_input_line(INPUT_LINE);
        assert_eq!(result, expected);
    }

    #[test]
    fn get_index_00() {
        let result = get_index(&get_valve_labels(), "AA");
        assert_eq!(result, 0);
    }

    #[test]
    fn get_index_05() {
        let result = get_index(&get_valve_labels(), "FF");
        assert_eq!(result, 5);
    }

    #[test]
    fn read_input_correctly() {
        let expected = Map {
            valve_labels: get_valve_labels(),
            valves: vec![

                Valve {
                    index: 1,
                    flow_rate: 13,
                    tunnel_indices: vec![2, 0],
                },
                Valve {
                    index: 0,
                    flow_rate: 0,
                    tunnel_indices: vec![3, 8, 1],
                },
                Valve {
                    index: 2,
                    flow_rate: 2,
                    tunnel_indices: vec![3, 1],
                },
                Valve {
                    index: 3,
                    flow_rate: 20,
                    tunnel_indices: vec![2, 0, 4],
                },
                Valve {
                    index: 4,
                    flow_rate: 3,
                    tunnel_indices: vec![5, 3],
                },
                Valve {
                    index: 5,
                    flow_rate: 0,
                    tunnel_indices: vec![4, 6],
                },
                Valve {
                    index: 6,
                    flow_rate: 0,
                    tunnel_indices: vec![5, 7],
                },
                Valve {
                    index: 7,
                    flow_rate: 22,
                    tunnel_indices: vec![6],
                },
                Valve {
                    index: 8,
                    flow_rate: 0,
                    tunnel_indices: vec![0, 9],
                },
                Valve {
                    index: 9,
                    flow_rate: 21,
                    tunnel_indices: vec![8],
                },
            ],
        };
        let result = read_input(INPUT);
        assert_eq!(result.valve_labels, expected.valve_labels);
        assert_eq!(result.valves.len(), expected.valves.len());
        for index in 0..result.valves.len() {
            let expected_valve = &expected.valves[index];
            let result_valve = &result.valves[index];
            assert_eq!(result_valve.index, expected_valve.index);
            assert_eq!(result_valve.flow_rate, expected_valve.flow_rate);
            assert_eq!(result_valve.tunnel_indices, expected_valve.tunnel_indices);
        }
    }
}
