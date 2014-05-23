public class MovingAction {

  public MovingAction () {
    dr = 0;
    dc = 0;
  }

  public MovingAction (int dr, int dc) {
    this.dr = dr;
    this.dc = dc;
  }

  public MovingAction (MovingAction other) {
    dr = other.dr;
    dc = other.dc;
  }

  public int dr, dc;

};

