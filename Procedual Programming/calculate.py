import math

def diameter_from_circumference(circumference):
    """根据周长计算直径"""
    return circumference / math.pi

def circumference_from_diameter(diameter):
    """根据直径计算周长"""
    return diameter * math.pi

def calculate_distances_and_periods(planets):
    calculated_data = []
    
    for planet in planets:
        distance_from_sun = planet.get('DistanceFromSun')
        orbital_period = planet.get('OrbitalPeriod')
        
        if distance_from_sun is None and orbital_period is not None:
            # Calculate DistanceFromSun from OrbitalPeriod
            distance_from_sun = (orbital_period ** 2) ** (1. / 3.)
        elif distance_from_sun is not None and orbital_period is None:
            # Calculate OrbitalPeriod from DistanceFromSun
            orbital_period = (distance_from_sun ** 3) ** 0.5
        
        calculated_data.append({
            'Name': planet.get('Name', 'Unknown'),
            'DistanceFromSun': distance_from_sun,
            'OrbitalPeriod': orbital_period
        })
    
    return calculated_data

def calculate_diameter_and_circumference(planets):
    calculated_data = []
    
    for planet in planets:
        diameter = planet.get('Diameter')
        circumference = planet.get('Circumference')
        
        if diameter and not circumference:
            # Calculate Circumference from Diameter
            circumference = circumference_from_diameter(diameter)
        elif circumference and not diameter:
            # Calculate Diameter from Circumference
            diameter = diameter_from_circumference(circumference)
        
        calculated_data.append({
            'Name': planet.get('Name', 'Unknown'),
            'Diameter': diameter,
            'Circumference': circumference
        })
    
    return calculated_data

def calculate_moon_diameter_and_circumference(moons):
    calculated_data = []
    
    for moon in moons:
        diameter = moon.get('Diameter')
        circumference = moon.get('Circumference')
        
        if diameter and not circumference:
            # Calculate Circumference from Diameter
            circumference = circumference_from_diameter(diameter)
        elif circumference and not diameter:
            # Calculate Diameter from Circumference
            diameter = diameter_from_circumference(circumference)
        
        calculated_data.append({
            'Name': moon.get('Name', 'Unknown'),
            'Diameter': diameter,
            'Circumference': circumference
        })
    
    return calculated_data


def calculate_volume(diameter):
    radius = diameter / 2
    volume = (4/3) * math.pi * (radius ** 3)
    return volume