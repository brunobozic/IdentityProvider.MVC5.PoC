using Module.CrossCutting;

namespace IdentityProvider.Repository.EFCore.OracleSequence
{
    public static class OracleSequenceHelper
    {
        // SELECT SMS_CARD.ULAZNA_PORUKA_ARHIVA_SEQ.NEXTVAL FROM dual;
        public static int GetNextSequenceValue(SequenceOf sequenceType /*, IHacEncDataContext context*/)
        {
            //try
            //{
            //    IList<decimal> sequence;
            //    switch (sequenceType)
            //    {
            //        case SequenceOf.SMS_ULAZNA_PORUKA_ARHIVA:
            //            sequence =
            //                context.GetDatabase().SqlQuery<decimal>("SELECT " +
            //                                                        "SMS_CARD." +
            //                                                        "ULAZNA_PORUKA_ARHIVA_SEQ.NEXTVAL FROM DUAL")
            //                    .ToList();
            //            return Convert.ToInt32(sequence[0]);
            //        case SequenceOf.SMS_ULAZNA_PORUKA:
            //            sequence =
            //                context.GetDatabase().SqlQuery<decimal>("SELECT " +
            //                                                        "SMS_CARD." +
            //                                                        "ULAZNA_PORUKA_SEQ.NEXTVAL FROM DUAL").ToList();
            //            return Convert.ToInt32(sequence[0]);

            //        default:
            //            throw new ArgumentException("Unknown sequence type: " + sequenceType);
            //    }
            //}
            //catch (Exception seqEx)
            //{
            //    LogFactory.GetLogger().LogFatal(new object(), "Null sequence", seqEx);
            //    throw new Exception("Null sequence");
            //}

            return 1;
        }
    }
}