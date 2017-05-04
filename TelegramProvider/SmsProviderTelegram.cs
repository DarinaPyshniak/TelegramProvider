using System.Linq;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Contacts;
using TeleSharp.TL.Messages;
using TLSharp.Core;

namespace TelegramProvider {
    public class SmsProviderTelegram {
        private readonly string _apiHash = "********************************";
        private readonly int _apiId = 11111;
        private readonly TelegramClient _client;

        public SmsProviderTelegram() {
            _client = new TelegramClient(_apiId, _apiHash);
        }

        public async Task<bool> Connect() {
            return await _client.ConnectAsync();
        }

        public async Task<int?> GetUserIdByPhoneNumber(string phoneNumber, string lastName = "", string firstName = "") {
            var requestImportContacts = new TLRequestImportContacts {
                contacts = new TLVector<TLInputPhoneContact>()
            };
            requestImportContacts.contacts.lists.Add(new TLInputPhoneContact {
                phone = phoneNumber,
                first_name = firstName,
                last_name = lastName
            });
            var importedContact = await _client.SendRequestAsync<TLImportedContacts>(requestImportContacts);
            return (importedContact.users.lists.First() as TLUser)?.id;
        }

        public async Task<int?> GetUserIdFromContactList(string phoneNumber) {
            var contactList = await _client.GetContactsAsync();
            var user = contactList.users.lists
                                  .Where(x => x.GetType() == typeof (TLUser))
                                  .Cast<TLUser>()
                                  .FirstOrDefault(x => x.phone == phoneNumber);
            var id = user?.id;
            return id;
        }

        public async void SendMessage(int? userId, string message) {
            if (userId != null)
                await _client.SendMessageAsync(new TLInputPeerUser {user_id = (int) userId}, message);                
        }
    }
}