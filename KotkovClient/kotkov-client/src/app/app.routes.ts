import { Routes } from '@angular/router';
import { LoginPage } from './pages/login-page/login-page';
import { CoursesPage } from './pages/courses-page/courses-page';
import { ProfilePage } from './pages/profile-page/profile-page';
import { Layout } from './common-ui/layout/layout';
import { canActivateAuth } from './auth/access.guard';
import { TeachersPage } from './pages/teachers-page/teachers-page';
import { StudentsPage } from './pages/students-page/students-page';
import { MyCoursesPage } from './pages/my-courses-page/my-courses-page';

export const routes: Routes = [
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: LoginPage },
    {path: '', component: Layout,canActivate: [canActivateAuth], children: [
        {path: 'courses', component: CoursesPage},
        {path: 'profile', component: ProfilePage}, 
        {path: 'teachers', component: TeachersPage}, 
        {path: 'students', component: StudentsPage},
        {path: 'mycourses', component: MyCoursesPage}
    ],
    }
];
