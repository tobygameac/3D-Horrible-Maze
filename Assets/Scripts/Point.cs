public class Point {

  public Point () {
    this.r = 0;
    this.c = 0;
  }

  public Point (int r, int c) {
    this.r = r;
    this.c = c;
  }

  public Point (Point other) {
    r = other.r;
    c = other.c;
  }

  public int r, c;

};

