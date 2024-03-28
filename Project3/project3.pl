
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
% assuming the transfer takes at least 30 minutes
can_transfer(ArrivalTime, DepartureTime) :-
    TransferTime is ArrivalTime + 30,
    TransferTime =< DepartureTime.

% Check direct and connecting flights
find_route(Origin, Destination, Route) :-
    (   flight(FlightNumber, Origin, Destination, Departure, Arrival),
        Route = [[FlightNumber, Departure, Arrival]]
    ;   flight(Flight1, Origin, Transfer, Departure1, Arrival1),
        flight(Flight3, Transfer, Destination, Departure3, Arrival3),
        Origin \= Destination,
        Transfer \= Destination,
        can_transfer(Arrival1, Departure3),
        Route = [[Flight1, Departure1, Arrival1], [Flight3, Departure3, Arrival3]],
        \+ member(Transfer, [Origin, Destination])
    ;   flight(Flight1, Origin, Transfer1, Departure1, Arrival1),
        flight(Flight2, Transfer1, Transfer2, Departure2, Arrival2),
        flight(Flight3, Transfer2, Destination, Departure3, Arrival3),
        Origin \= Destination,
        Transfer1 \= Destination,
        Transfer2 \= Destination,
        can_transfer(Arrival1, Departure2),
        can_transfer(Arrival2, Departure3),
        Route = [[Flight1, Departure1, Arrival1], [Flight2, Departure2, Arrival2], [Flight3, Departure3, Arrival3]],
        \+ member(Transfer1, [Origin, Destination]),
        \+ member(Transfer2, [Origin, Destination, Transfer1])
    ).