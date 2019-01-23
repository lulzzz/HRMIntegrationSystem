export interface News {
  id: string;
  unitId: number;
  title: string;
  imageId?: number;
  text: string;
  date: Date;
  fromDate?: Date;
  toDate?: Date;
  author: string;
  email: string;
}
