using System.Collections.Generic;

public class DraugrStateLibrary
{
    private enum States
    {

    }

    DraugrStateMachine _context;
    Dictionary<States, DraugrBaseState> _states = new Dictionary<States, DraugrBaseState>();

    public DraugrStateLibrary(DraugrStateMachine currentContext)
    {
        _context = currentContext;
    }
}