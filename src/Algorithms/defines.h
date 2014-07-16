/* Seven Segment Optical Character Recognition Constant Definitions */

/*  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

/* Copyright (C) 2004-2013 Erik Auerswald <auerswal@unix-ag.uni-kl.de> */
/* Copyright (C) 2013 Cristiano Fontana <fontanacl@ornl.gov> */

#ifndef SSOCR2_DEFINES_H
#define SSOCR2_DEFINES_H

/* states */
#define FIND_DARK 0
#define FIND_LIGHT 1

#define DARK 0
#define LIGHT 1
#define UNKNOWN 2

/* segments
 *
 *  1     -
 * 2 3   | |
 *  4     -
 * 5 6   | |
 *  7     -
 *
 *  */
#define HORIZ_UP 1
#define VERT_LEFT_UP 2
#define VERT_RIGHT_UP 4
#define HORIZ_MID 8
#define VERT_LEFT_DOWN 16
#define VERT_RIGHT_DOWN 32
#define HORIZ_DOWN 64
#define ALL_SEGS 127

/* digits */
#define D_ZERO (ALL_SEGS & ~HORIZ_MID)
#define D_ONE (VERT_RIGHT_UP | VERT_RIGHT_DOWN)
#define D_TWO (ALL_SEGS & ~(VERT_LEFT_UP | VERT_RIGHT_DOWN))
#define D_THREE (ALL_SEGS & ~(VERT_LEFT_UP | VERT_LEFT_DOWN))
#define D_FOUR (ALL_SEGS & ~(HORIZ_UP | VERT_LEFT_DOWN | HORIZ_DOWN))
#define D_FIVE (ALL_SEGS & ~(VERT_RIGHT_UP | VERT_LEFT_DOWN))
#define D_SIX (ALL_SEGS & ~VERT_RIGHT_UP)
#define D_SEVEN (HORIZ_UP | VERT_RIGHT_UP | VERT_RIGHT_DOWN)
#define D_ALTSEVEN (VERT_LEFT_UP | D_SEVEN)
#define D_EIGHT ALL_SEGS
#define D_NINE (ALL_SEGS & ~VERT_LEFT_DOWN)
#define D_ALTNINE (ALL_SEGS & ~(VERT_LEFT_DOWN | HORIZ_DOWN))
#define D_UNKNOWN 0

/* a one is recognized by a height/width ratio > ONE_RATIO (as ints) */
#define ONE_RATIO 3

/* to find segment need # of pixels */
#define NEED_PIXELS 5

/* ignore # of pixels when checking a column fo black or white */
#define IGNORE_PIXELS 0

/* colors used by ssocr */
#define SSOCR_BLACK 0
#define SSOCR_WHITE 255

#endif /* SSOCR2_DEFINES_H */
