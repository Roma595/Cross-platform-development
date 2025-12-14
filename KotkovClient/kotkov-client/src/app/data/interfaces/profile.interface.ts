import { CourseShort } from "./course.interface";

export interface Profile {
  id: number
  firstName: string
  lastName: string
  phoneNumber: string | null
  role: 'Student' | 'Teacher';
  courses?: CourseShort[];
}
