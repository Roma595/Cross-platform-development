export interface Course {
  id: number 
  teacherId: number
  name: string
  totalPlaces: number
  startDate: Date
  endDate: Date
}

export interface CourseShort {
  name: string;
}

export type CoursesViewMode = 'cards' | 'table';
