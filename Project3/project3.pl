
flight(6711, bos, ord, 0815, 1005).
flight(211, lga, ord, 0700, 0830).
flight(203, lga, lax, 0730, 1335).
flight(92221, ewr, ord, 0800, 0920).
flight(2134, ord, sfo, 0930, 1345).
flight(954, phx, dfw, 1655, 1800).
flight(1176, sfo, lax, 1430, 1545).
flight(205, lax, lga, 1630, 2210).


% Where does the flight from PHX go?
destination_from_phx(Destination) :- flight(_, phx, Destination, _, _).

% Is there a flight to PHX?
flight_to_phx(FlightNumber) :- flight(FlightNumber, _, phx, _, _).

% What time is does the flight from BOS land?
landing_time_from_bos(LandingTime) :- flight(_, bos, _, _, LandingTime).

% Does the flight from ORD to SFO depart after the flight from EWR to ORD lands?
depart_after_arrival(DepartureTime) :-
    flight(_, ewr, ord, _, ArrivalTime),
    flight(_, ord, sfo, DepartureTime, _),
    DepartureTime > ArrivalTime.

% What time do the flights to ORD arrive?
arrival_times_to_ord(ArrivalTime) :- flight(_, _, ord, _, ArrivalTime).



% What are all the ways to get from LGA to LAX?
direct_route_lga_lax(lga, lax, FlightNumber, DepartureTime, ArrivalTime) :-
    flight(FlightNumber, lga, lax, DepartureTime, ArrivalTime).

connection_route_lga_lax(FlightNumber1, FlightNumber2, FlightNumber3, DepartureTime1, ArrivalTime3) :-
    flight(FlightNumber1, lga, ord, DepartureTime1, ArrivalTime1),
    flight(FlightNumber2, ord, sfo, DepartureTime2, ArrivalTime2),
    flight(FlightNumber3, sfo, lax, DepartureTime3, ArrivalTime3),
    ArrivalTime1 < DepartureTime2,
    ArrivalTime2 < DepartureTime3.


route_lga_lax(Route, DepartureTime, ArrivalTime) :-
    direct_route_lga_lax(lga, lax, FlightNumber, DepartureTime, ArrivalTime),
    Route = [FlightNumber],
    writeln('Direct flight:').

route_lga_lax(Route, DepartureTime, ArrivalTime) :-
    connection_route_lga_lax(FlightNumber1, FlightNumber2, FlightNumber3, DepartureTime, ArrivalTime),
    Route = [FlightNumber1, FlightNumber2, FlightNumber3],
    writeln('Connection flight:').

