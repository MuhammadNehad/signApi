using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace locationRecordeapi
{
    public class EmplyeesAttendings
    {
        public int Id  {get;set;}
        public int empKey  {get;set;}
        public int locationKey {get;set;}
        public DateTime atdt    {get;set;}
        public bool entering    {get;set;}
        public Int64 leaveAfter  {get;set;}
        public int aemplyeeId  {get;set;}
        public string email   {get;set;}
        public string name    {get;set;}
        public string phone   {get;set;}
        public int locationIdfk    {get;set;}
        public string empCode {get;set;}
        public int role    {get;set;}
        public int locationIdPK    {get;set;}
        public double latitude    {get;set;}
        public double lngtude {get;set;}
        public string address {get;set;}
        public int area    {get;set;}
        public Int64 waitingTime {get;set;}
        public int parentLocation  {get;set;}
        public bool isParent    {get;set;}
        public int roleId  {get;set;}
        public string mrole_name  {get;set;}
        public int parent_id   {get;set;}

    }
}
