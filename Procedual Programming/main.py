import SolarSystem
from calculate import diameter_from_circumference, circumference_from_diameter, calculate_distances_and_periods, calculate_diameter_and_circumference, calculate_moon_diameter_and_circumference, calculate_volume




def print_sun_info():
    sun = SolarSystem.sun

    name = sun.get('Name', 'Sol')  
    diameter = sun.get('Diameter')
    circumference = sun.get('Circumference')

    if diameter and not circumference:
        circumference = circumference_from_diameter(diameter)
    elif circumference and not diameter:
        diameter = diameter_from_circumference(circumference)

    # calculate volume of sun
    sun_volume = calculate_volume(diameter)

    # Print Sun information
    print(f"Sun: {name}")
    print(f"Diameter: {diameter:,} km")
    print(f"Circumference: {circumference:,} km")
    print()  
    print("...")  
    print()  

def print_planet_info():
    planets = SolarSystem.sun.get('Planets', [])
    distances_and_periods = calculate_distances_and_periods(planets)
    diameter_and_circumference = calculate_diameter_and_circumference(planets)
    
    for i, data in enumerate(distances_and_periods):
        planet_name = data['Name']
        distance_from_sun = data['DistanceFromSun']
        orbital_period = data['OrbitalPeriod']
        diameter = diameter_and_circumference[i]['Diameter']
        circumference = diameter_and_circumference[i]['Circumference']
        moons = planets[i].get('Moons', [])  # Get moons from the planet data
        
        print(f"Planet: {planet_name}")
        print(f"Distance from sun: {distance_from_sun:.2f} au")
        print(f"Orbital period: {orbital_period:.2f} yr")
        print(f"Diameter: {diameter:,} km")
        print(f"Circumference: {circumference:,} km")
        
        # Check if the planet has moons
        if moons:
            print("Moons:")
            # Calculate moon diameter and circumference
            moon_data = calculate_moon_diameter_and_circumference(moons)
            for moon in moon_data:
                moon_name = moon['Name']
                moon_diameter = moon['Diameter']
                moon_circumference = moon['Circumference']
                print(f"- {moon_name}:")
                print(f"  - Diameter: {moon_diameter:,} km")
                print(f"  - Circumference: {moon_circumference:,} km")
        
        print()

def main():
    print_sun_info()
    print_planet_info()

    # 计算太阳和行星的体积
    sun_diameter = SolarSystem.sun.get('Diameter')
    sun_volume = calculate_volume(sun_diameter)
    planets = SolarSystem.sun.get('Planets', [])
    total_planets_volume = sum([calculate_volume(planet.get('Diameter', 0)) for planet in planets])

    # 判断是否所有行星的体积之和大于太阳的体积
    planets_can_fit_in_sun = total_planets_volume > sun_volume
    print('Sun volume: ', sun_volume)
    print('Total planets volume:', total_planets_volume)
    print("All the planets’ volumes added together could fit in the Sun:", planets_can_fit_in_sun)

if __name__ == "__main__":
    main()
