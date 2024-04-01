
flight(6711, bos, ord, 0815, 1005).
flight(211, lga, ord, 0700, 0830).
flight(203, lga, lax, 0730, 1335).
flight(92221, ewr, ord, 0800, 0920).
flight(2134, ord, sfo, 0930, 1345).
flight(954, phx, dfw, 1655, 1800).
flight(1176, sfo, lax, 1430, 1545).
flight(205, lax, lga, 1630, 2210).
flight(111, lga, bos, 0645, 0745).
flight(222, bos, ewr, 0750, 0845).

% 1.Where does the flight from PHX go?
destination_from_phx(Destination) :- flight(_, phx, Destination, _, _).

% 2.Is there a flight to PHX?
flight_to_phx(FlightNumber) :- flight(FlightNumber, _, phx, _, _).

% 3.What time is does the flight from BOS land?
landing_time_from_bos(FlightNumber, LandingTime) :-
    flight(FlightNumber, bos, _, _, LandingTime).

% 4.Does the flight from ORD to SFO depart after the flight from EWR to ORD lands?
print_depart_after_arrival :-
    flight(_, ewr, ord, _, ArrivalTime),
    flight(FlightNumber, ord, sfo, DepartureTime, _),
    DepartureTime > ArrivalTime,
    format('Flight ~w from ORD to SFO departs at ~w, which is after the arrival from EWR to ORD.', [FlightNumber, DepartureTime]),
    nl.

% 5.What time do the flights to ORD arrive?
arrival_times_to_ord(FlightNumber, ArrivalTime) :-
    flight(FlightNumber, _, ord, _, ArrivalTime).


% 6.What are all the ways to get from LGA to LAX?
% assuming the transfer takes at least 30 minutes
can_transfer(ArrivalTime, DepartureTime) :-
    TransferTime is ArrivalTime + 30,
    TransferTime =< DepartureTime.

% Check direct and connecting flights
find_route(Origin, Destination, Route) :-
    (   % 直达航班
        flight(FlightNumber, Origin, Destination, Departure, Arrival),
        Route = ['Direct_flights', [flight_number: FlightNumber, departure_time: Departure, arrival_time: Arrival]]
    ;   % 一次换乘
        flight(Flight1, Origin, Transfer, Departure1, Arrival1),
        flight(Flight2, Transfer, Destination, Departure2, Arrival2),
        Origin \= Destination,
        Transfer \= Destination,
        can_transfer(Arrival1, Departure2),
        Route = ['Connecting flights', [flight_number: Flight1, departure_time: Departure1, arrival_time: Arrival1], [flight_number: Flight2, departure_time: Departure2, arrival_time: Arrival2]]
    ;   % 两次换乘
        flight(Flight1, Origin, Transfer1, Departure1, Arrival1),
        flight(Flight2, Transfer1, Transfer2, Departure2, Arrival2),
        flight(Flight3, Transfer2, Destination, Departure3, Arrival3),
        Origin \= Destination,
        Transfer1 \= Destination,
        Transfer2 \= Destination,
        can_transfer(Arrival1, Departure2),
        can_transfer(Arrival2, Departure3),
        Route = ['Connecting flights', [flight_number: Flight1, departure_time: Departure1, arrival_time: Arrival1], [flight_number: Flight2, departure_time: Departure2, arrival_time: Arrival2], [flight_number: Flight3, departure_time: Departure3, arrival_time: Arrival3]]
    ;   % 三次换乘
        flight(Flight1, Origin, Transfer1, Departure1, Arrival1),
        flight(Flight2, Transfer1, Transfer2, Departure2, Arrival2),
        flight(Flight3, Transfer2, Transfer3, Departure3, Arrival3),
        flight(Flight4, Transfer3, Destination, Departure4, Arrival4),
        Origin \= Destination,
        Transfer1 \= Destination, Transfer2 \= Destination, Transfer3 \= Destination,
        can_transfer(Arrival1, Departure2), can_transfer(Arrival2, Departure3), can_transfer(Arrival3, Departure4),
        Route = ['Connecting flights', [flight_number: Flight1, departure_time: Departure1, arrival_time: Arrival1], [flight_number: Flight2, departure_time: Departure2, arrival_time: Arrival2], [flight_number: Flight3, departure_time: Departure3, arrival_time: Arrival3], [flight_number: Flight4, departure_time: Departure4, arrival_time: Arrival4]]
    ).