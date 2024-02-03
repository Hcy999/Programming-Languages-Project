import math
from SolarSystem import sun as solar_system_data

# CelestialObject class
class CelestialObject:
    def __init__(self, name, diameter=None, circumference=None):
        self.name = name
        self.diameter = diameter
        self.circumference = circumference
        self.volume = self.calculate_volume()

    def calculate_diameter_from_circumference(self):
        if self.circumference is not None and self.diameter is None:
            self.diameter = self.circumference / math.pi
        return self.diameter

    def calculate_circumference_from_diameter(self):
        if self.diameter is not None and self.circumference is None:
            self.circumference = self.diameter * math.pi
        return self.circumference

    def calculate_volume(self):
        if self.diameter is not None:
            radius = self.diameter / 2
            return (4/3) * math.pi * (radius ** 3)
        else:
            return 0

# Sun class
class Sun(CelestialObject):
    def __init__(self, name, diameter):
        super().__init__(name, diameter)
        self.planets = []
        self.circumference = self.calculate_circumference_from_diameter()

    def sum_volume_of_planets(self):
        return sum(planet.volume for planet in self.planets)

# Planet class
class Planet(CelestialObject):
    def __init__(self, name, diameter=None, circumference=None, distance_from_sun=None, orbital_period=None, moons=None):
        super().__init__(name, diameter, circumference)
        self.diameter = diameter
        self.circumference = circumference
        self.distance_from_sun = distance_from_sun
        self.orbital_period = orbital_period
        self.moons = [moon if isinstance(moon, Moon) else Moon(**moon) for moon in (moons or [])]

    def calculate_orbit_time_from_distance(self):
        if self.distance_from_sun is not None and self.orbital_period is None:
            self.orbital_period = math.sqrt(self.distance_from_sun ** 3)
        return self.orbital_period

    def calculate_distance_from_orbit_time(self):
        if self.orbital_period is not None and self.distance_from_sun is None:
            self.distance_from_sun = self.orbital_period ** (2/3)
        return self.distance_from_sun

# Moon class
class Moon(CelestialObject):
    pass

# SolarSystem class
class SolarSystem:
    def __init__(self, data):
        self.sun = Sun(data['Name'], data['Diameter'])
        self.planets = [self.create_planet(**planet) for planet in data.get('Planets', [])]

    def create_planet(self, **kwargs):
        planet_kwargs = {
            'name': kwargs.get('Name'),
            'diameter': kwargs.get('Diameter'),
            'circumference': kwargs.get('Circumference'),
            'distance_from_sun': kwargs.get('DistanceFromSun'),
            'orbital_period': kwargs.get('OrbitalPeriod'),
            'moons': [self.create_moon(**moon) for moon in kwargs.get('Moons', [])]
        }

        planet_kwargs = {k: v for k, v in planet_kwargs.items() if v is not None}
        planet = Planet(**planet_kwargs)
        if planet.diameter is None:
            planet.diameter = planet.calculate_diameter_from_circumference()
        elif planet.circumference is None:
            planet.circumference = planet.calculate_circumference_from_diameter()        
        if planet.distance_from_sun is None:
            planet.distance_from_sun = planet.calculate_distance_from_orbit_time()
        elif planet.orbital_period is None:
            planet.orbital_period = planet.calculate_orbit_time_from_distance()        
        return planet
    
    def create_moon(self, **kwargs):
        moon_kwargs = {
            'name': kwargs.get('Name'),
            'diameter': kwargs.get('Diameter'),
            'circumference': kwargs.get('Circumference')
        }
        moon_kwargs = {k: v for k, v in moon_kwargs.items() if v is not None}
        moon = Moon(**moon_kwargs)
        if moon.diameter is None:
            moon.diameter = moon.calculate_diameter_from_circumference()
        elif moon.circumference is None:
            moon.circumference = moon.calculate_circumference_from_diameter()        
        return moon


# Main function
def main():
    solar_system = SolarSystem(solar_system_data)
    print(f"Sun: {solar_system.sun.name}")
    print(f"Diameter: {solar_system.sun.diameter:,} km")
    print(f"Circumference: {solar_system.sun.circumference:,.0f} km")
    sun_volume = solar_system.sun.volume
    total_volume = 0  
    
    print("...")
    
    for planet in solar_system.planets:
        print(f"Planet: {planet.name}")
        print(f"  Distance from sun: {planet.distance_from_sun} AU")
        print(f"  Orbital period: {planet.orbital_period} years")
        print(f"  Diameter: {planet.diameter:,} km" )
        print(f"  Circumference: {planet.circumference:,.0f} km")
        planet_volume = planet.volume  
        total_volume += planet_volume 
        for moon in planet.moons:
            print(f"    Moon: {moon.name}")
            print(f"      Diameter: {moon.diameter:,} km")
            print(f"      Circumference: {moon.circumference:,.0f} km")
            moon_volume = moon.volume
            total_volume += moon_volume  
        print("...")
    
    print(f"Sun Volume: {sun_volume:,.0f} cubic km")
    print(f"Total Volume of Planets and Moons: {total_volume:,.0f} cubic km")
    
    can_fit_in_sun = total_volume > sun_volume
    print(f"All the planets’ volumes added together could fit in the Sun: {can_fit_in_sun}")

if __name__ == "__main__":
    main()
