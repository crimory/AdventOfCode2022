use std::collections::HashMap;

#[derive(Clone, Debug, PartialEq)]
pub struct Valve {
    pub name: String,
    pub flow_rate: u32,
    pub tunnels: Vec<String>,
    pub opened: bool,
}

fn read_input_line(input: &str) -> Valve {
    let parts = input
        .split("has flow rate=")
        .flat_map(|s| s.trim().split("; tunnels lead to valves"))
        .flat_map(|s| s.trim().split("; tunnel leads to valve"))
        .flat_map(|s| s.trim().split("Valve"))
        .map(|s| s.trim())
        .filter(|s| !s.is_empty())
        .collect::<Vec<&str>>();
    Valve {
        name: parts[0].to_owned(),
        flow_rate: parts[1].parse::<u32>().unwrap(),
        tunnels: parts[2].split(',').map(|s| s.trim().to_owned()).collect(),
        opened: false,
    }
}

pub fn read_input(input: &str) -> HashMap<String, Valve> {
    let mut output = HashMap::new();
    input.lines().map(read_input_line).for_each(|v| {
        output.insert((v.name).to_owned(), v);
    });
    output
}

#[cfg(test)]
pub mod tests {
    use super::*;

    const INPUT_LINE: &str = "\
Valve AA has flow rate=0; tunnels lead to valves DD, II, BB";
    pub const INPUT: &str = "\
Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II";

    #[test]
    fn read_line_correctly() {
        let expected = Valve {
            name: "AA".to_owned(),
            flow_rate: 0,
            tunnels: vec!["DD".to_owned(), "II".to_owned(), "BB".to_owned()],
            opened: false,
        };
        let result = read_input_line(INPUT_LINE);
        assert_eq!(result, expected);
    }

    #[test]
    fn read_input_correctly() {
        let expected = HashMap::from([
            (
                "AA".to_owned(),
                Valve {
                    name: "AA".to_owned(),
                    flow_rate: 0,
                    tunnels: vec!["DD".to_owned(), "II".to_owned(), "BB".to_owned()],
                    opened: false,
                },
            ),
            (
                "BB".to_owned(),
                Valve {
                    name: "BB".to_owned(),
                    flow_rate: 13,
                    tunnels: vec!["CC".to_owned(), "AA".to_owned()],
                    opened: false,
                },
            ),
            (
                "CC".to_owned(),
                Valve {
                    name: "CC".to_owned(),
                    flow_rate: 2,
                    tunnels: vec!["DD".to_owned(), "BB".to_owned()],
                    opened: false,
                },
            ),
            (
                "DD".to_owned(),
                Valve {
                    name: "DD".to_owned(),
                    flow_rate: 20,
                    tunnels: vec!["CC".to_owned(), "AA".to_owned(), "EE".to_owned()],
                    opened: false,
                },
            ),
            (
                "EE".to_owned(),
                Valve {
                    name: "EE".to_owned(),
                    flow_rate: 3,
                    tunnels: vec!["FF".to_owned(), "DD".to_owned()],
                    opened: false,
                },
            ),
            (
                "FF".to_owned(),
                Valve {
                    name: "FF".to_owned(),
                    flow_rate: 0,
                    tunnels: vec!["EE".to_owned(), "GG".to_owned()],
                    opened: false,
                },
            ),
            (
                "GG".to_owned(),
                Valve {
                    name: "GG".to_owned(),
                    flow_rate: 0,
                    tunnels: vec!["FF".to_owned(), "HH".to_owned()],
                    opened: false,
                },
            ),
            (
                "HH".to_owned(),
                Valve {
                    name: "HH".to_owned(),
                    flow_rate: 22,
                    tunnels: vec!["GG".to_owned()],
                    opened: false,
                },
            ),
            (
                "II".to_owned(),
                Valve {
                    name: "II".to_owned(),
                    flow_rate: 0,
                    tunnels: vec!["AA".to_owned(), "JJ".to_owned()],
                    opened: false,
                },
            ),
            (
                "JJ".to_owned(),
                Valve {
                    name: "JJ".to_owned(),
                    flow_rate: 21,
                    tunnels: vec!["II".to_owned()],
                    opened: false,
                },
            ),
        ]);
        let result = read_input(INPUT);
        assert_eq!(result.len(), expected.len());
        for (key, valve) in result {
            assert_eq!(expected[&key], valve);
        }
    }
}
