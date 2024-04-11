# Realtime Satellite Visualiser

This project is a Unity 2018.3.0 application that provides a realtime visualisation of satellites using data from [Celestrak](https://celestrak.org/NORAD/elements/gp.php?GROUP=starlink&FORMAT=tle) and functions from [Vallado's Fundamentals of Astrodynamics and Applications](https://astrobooks.com/vallado5hb.aspx).

## Overview

This project aims to provide a visually immersive experience of tracking satellites in real-time. Using Unity 2018.3.0, it takes datasets from Celestrak focusing on Starlink satellites, and uses Vallado's implementation of SGP4 to accurately model the motion of these satellites in real time.

## Getting Started

To get started with the project, follow these steps:

1. Clone this repository to your local machine.

2. Open the Unity 2018.3.0 project in your Unity Editor.

3. Explore the project and run the application within the Unity Editor.

4. (optional) update the data by replacing the gp.txt in Assets/StreamingAssets with an up-to-date copy of the Celestrak file.

## Dependencies

- Unity 2018.3.0 or higher

- (optional) Internet connection for fetching satellite data from Celestrak
## License

This project is licensed under the [MIT License](LICENSE).

## Acknowledgements

- Celestrak for providing satellite data.

- David A. Vallado for his work on astrodynamics.

- Contributors to Unity for their development platform.

## Contact

For any inquiries or suggestions, feel free to contact ttom795@aucklanduni.ac.nz.
