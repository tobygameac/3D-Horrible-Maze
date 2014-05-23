public class Point {

  public Point () {
    r = 0;
    c = 0;
  }

  public Point (int r, int c) {
    this.r = r;
    this.c = c;
  }

  public Point (Point other) {
    r = other.r;
    c = other.c;
  }

  public bool equal (Point other) {
    return (r == other.r) && (c == other.c);
  }

  public int r, c;

};

